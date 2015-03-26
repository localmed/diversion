using System;
using Microsoft.Build.Evaluation;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Text;

namespace Diversion.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = CmdLine.CommandLine.Parse<Options>();
            options.Target = options.Target ?? GetProjectFilePath();
            if (!Regex.IsMatch(options.Target, ".*(.dll|.exe)$"))
                BuildTarget(options);
            options.NuGetPackageId = options.NuGetPackageId ?? Path.GetFileNameWithoutExtension(options.Target);
            switch (options.ReleaseSource)
            {
                case ReleaseSource.Git:
                    GetReleaseUsingGit(options);
                    break;
                case ReleaseSource.NuGet:
                    GetReleaseUsingNuGet(options);
                    break;
                case ReleaseSource.Auto:
                    if (options.ReleasedPath == null && !(GetReleaseUsingNuGet(options) || GetReleaseUsingGit(options)) || !File.Exists(options.ReleasedPath))
                    {
                        CmdLine.CommandLine.WriteLineColor(ConsoleColor.Red, "Could not obtain released assembly.");
                        Console.ReadKey();
                        return;
                    }
                    break;
            }
            Console.WriteLine("Divining diversion of target from released assembly...");
            var diversion = new AssemblyDiversionDiviner().Divine(options.ReleasedPath, options.Target);
            var version = new NextVersion().Determine(diversion);
            if (version > diversion.New.Version)
                if (!options.Correct)
                {
                    CmdLine.CommandLine.WriteLineColor(ConsoleColor.DarkRed, "v{0} [Currently v{1}]", version, diversion.New.Version);
                }
                else
                {
                    Console.WriteLine("Updating assembly info in target project...");
                    File.WriteAllText(options.AssemblyInfoFile, string.Format(Regex.Replace(File.ReadAllText(options.AssemblyInfoFile), "(Assembly(?:File|Informational)?Version)\\b*\\(\b*\"([0-9.]*)\"\\b*\\)", "$1(\"{0}\")"), version));
                    CmdLine.CommandLine.WriteLineColor(ConsoleColor.DarkCyan, "v{0} [Was v{1}]", version, diversion.New.Version);
                }
            else if (version == diversion.New.Version)
                CmdLine.CommandLine.WriteLineColor(ConsoleColor.Green, "v{0}", version);
            else
                CmdLine.CommandLine.WriteLineColor(ConsoleColor.DarkGreen, "v{0} [Determined v{1}]", diversion.New.Version, version);
        }

        private static void BuildTarget(Options options)
        {
            Console.WriteLine("Building target project...");
            options.ProjectDirectory = Path.GetDirectoryName(options.Target);
            options.SolutionDirectory = Path.GetDirectoryName(options.ProjectDirectory);
            ProjectCollection projects = new ProjectCollection(ToDictionary(options.BuildProperties));
            var project = projects.LoadProject(options.Target);
            if (!project.Build())
                CmdLine.CommandLine.WriteLineColor(ConsoleColor.Red, "Could not build target project.");
            options.Target = Path.Combine(project.DirectoryPath, project.GetPropertyValue("OutputPath"), project.GetPropertyValue("TargetFileName"));
        }

        private static IDictionary<string, string> ToDictionary(string properties)
        {
            return properties.Split(';')
                .Where(property => property.Contains("="))
                .Select(property => property.Split('='))
                .ToDictionary(property => property[0].Trim(), property => property[1].Trim());
        }

        private static string GetProjectFilePath()
        {
            string[] projectExtensions = new[] { ".csproj", ".vbproj", ".proj" };
            var projectFile = Directory.EnumerateFiles(Environment.CurrentDirectory).Select(file => new FileInfo(file)).FirstOrDefault(file => file.Name.Equals(file.Directory.Name) && projectExtensions.Contains(file.Extension)) ??
                Directory.EnumerateFiles(Environment.CurrentDirectory).Select(file => new FileInfo(file)).FirstOrDefault(file => projectExtensions.Contains(file.Extension));
            return projectFile == null ? null : projectFile.FullName;
        }

        private static string GetDiversionWorkingDirectory()
        {
            string working = Path.Combine(Environment.CurrentDirectory, ".diversion");
            if (!Directory.Exists(working))
                Directory.CreateDirectory(working);
            return working;
        }

        private static bool GetReleaseUsingGit(Options options)
        {
            Console.WriteLine("Attempting to reconstruct released assembly from git repo...");
            return false;
        }

        private static bool GetReleaseUsingNuGet(Options options)
        {
            string working = GetDiversionWorkingDirectory();
            if (!IsNuGetAvailable(working))
                return false;

            Console.WriteLine("Attempting to download released assembly from nuget...");
            if (options.NuGetConfigPath == null && File.Exists(Path.Combine(options.SolutionDirectory ?? Environment.CurrentDirectory, "nuget.config")))
                options.NuGetConfigPath = Path.Combine(options.SolutionDirectory ?? Environment.CurrentDirectory, "nuget.config");

            string packages = Path.Combine(working, "packages");
            string references = Path.Combine(working, "references");
            if (!Directory.Exists(packages))
                Directory.CreateDirectory(packages);
            if (!Directory.Exists(references))
                Directory.CreateDirectory(references);
            var argBuilder = new StringBuilder();
            argBuilder.AppendFormat("install -OutputDirectory \"{0}\" ", packages);
            if (options.NuGetConfigPath != null)
                argBuilder.AppendFormat("-ConfigFile \"{0}\" ", options.NuGetConfigPath);
            argBuilder.AppendFormat("{0} {1}", options.NuGetPackageId, options.NuGetPackageVersion);
            Process.Start(new ProcessStartInfo
            {
                UseShellExecute = false,
                FileName = Path.Combine(working, "nuget.exe"),
                Arguments = argBuilder.ToString(),
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            }).WaitForExit();
            foreach (string assembly in Directory.EnumerateDirectories(packages).Select(path => Path.Combine(path, "lib")).Where(path => Directory.Exists(path)).SelectMany(path => Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories).Concat(Directory.EnumerateFiles(path, "*.exe", SearchOption.AllDirectories))))
                File.Copy(assembly, Path.Combine(references, Path.GetFileName(assembly)), true);
            options.ReleasedPath = Path.Combine(references, Path.GetFileName(options.Target));
            return File.Exists(options.ReleasedPath);
        }

        private static bool IsNuGetAvailable(string path)
        {
            string nugetPath = Path.Combine(path, "nuget.exe");
            if (File.Exists(nugetPath))
                return true;
            try
            {
                Console.WriteLine("Downloading latest version of nuget.exe...");
                new WebClient().DownloadFile("https://www.nuget.org/nuget.exe", Path.Combine(path, "nuget.exe"));
                return true;
            }
            catch (Exception ex)
            {
                CmdLine.CommandLine.WriteLineColor(ConsoleColor.Red, "Problem downloading nuget.exe {0}", ex.Message);
                return false;
            }
        }

    }

    class Options
    {

        [CmdLine.CommandLineParameter(Name = "Target", ParameterIndex = 1)]
        public string Target { get; set; }

        [CmdLine.CommandLineParameter("p", Name = "BuildProperties", Default = "Configuration=Release")]
        public string BuildProperties { get; set; }

        [CmdLine.CommandLineParameter("r", Name = "ReleasedPath")]
        public string ReleasedPath { get; set; }

        [CmdLine.CommandLineParameter("c", Name = "Correct", Default = false)]
        public bool Correct { get; set; }

        [CmdLine.CommandLineParameter("a", Name = "AssemblyInfoFile", Default = "Properties\\AssemblyInfo.cs")]
        public string AssemblyInfoFile { get; set; }

        [CmdLine.CommandLineParameter("s", Name = "ReleaseSource", Default = ReleaseSource.Auto)]
        public ReleaseSource ReleaseSource { get; set; }

        [CmdLine.CommandLineParameter(Command = "GitReleaseBranch", Default = "origin/master")]
        public string GitReleaseBranch { get; set; }

        [CmdLine.CommandLineParameter(Command = "GitPath")]
        public string GitPath { get; set; }

        [CmdLine.CommandLineParameter(Command = "NuGetConfigPath")]
        public string NuGetConfigPath { get; set; }

        [CmdLine.CommandLineParameter(Command = "NuGetPackageId")]
        public string NuGetPackageId { get; set; }

        [CmdLine.CommandLineParameter(Command = "NuGetPackageVersion")]
        public string NuGetPackageVersion { get; set; }

        internal string SolutionDirectory { get; set; }

        internal string ProjectDirectory { get; set; }
    }


    enum ReleaseSource
    {
        Auto,
        NuGet,
        Git,
        File        
    }
}
