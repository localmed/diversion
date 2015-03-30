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
            try
            {
                var options = CmdLine.CommandLine.Parse<Options>();
                options.Target = options.Target ?? GetProjectFilePath(Environment.CurrentDirectory);
                if (options.Target == null)
                {
                    CmdLine.CommandLine.WriteLineColor(ConsoleColor.Red, "A valid target could not be found in the working directory.");
                    return;
                }
                options.ProjectDirectory = Path.GetDirectoryName(options.Target);
                options.WorkingDirectory = Path.Combine(options.ProjectDirectory, ".diversion");
                options.AssemblyInfoFile = Path.Combine(options.ProjectDirectory, options.AssemblyInfoFile);
                if (!Regex.IsMatch(options.Target, ".*(.dll|.exe)$"))
                    BuildTarget(options);
                switch (options.ReleaseSource)
                {
                    case ReleaseSource.Git:
                        GetReleaseUsingGit(options);
                        break;
                    case ReleaseSource.NuGet:
                        GetReleaseUsingNuGet(options);
                        break;
                    case ReleaseSource.Auto:
                        if (options.ReleasedPath == null && !GetReleaseUsingNuGet(options))
                            GetReleaseUsingGit(options);
                        break;
                }
                if (options.ReleasedPath != null)
                    DivineDiversion(options);
                else
                    CmdLine.CommandLine.WriteLineColor(ConsoleColor.Red, "Could not acquire the last deployed release.");
            }
            catch (CmdLine.CommandLineException exception)
            {
                Console.WriteLine(exception.ArgumentHelp.Message);
                Console.WriteLine(exception.ArgumentHelp.GetHelpText(Console.BufferWidth));
            }

#if DEBUG
            CmdLine.CommandLine.Pause();
#endif
            return;
        }

        private static void DivineDiversion(Options options)
        {
            try
            {
                Console.WriteLine("Divining the semantic version of the target assembly based on its diversion from the last deployed release...");
                var diversion = new AssemblyDiversionDiviner().Divine(options.ReleasedPath, options.Target);
                var version = new NextVersion().Determine(diversion);
                if (version > diversion.New.Version)
                    if (!options.Correct)
                    {
                        CmdLine.CommandLine.WriteLineColor(ConsoleColor.DarkRed, "v{0} [Currently v{1}]", version, diversion.New.Version);
                    }
                    else
                    {
                        Console.WriteLine("Updating the assembly info in the target project...");
                        File.WriteAllText(options.AssemblyInfoFile, string.Format(Regex.Replace(File.ReadAllText(options.AssemblyInfoFile), "(Assembly(?:File|Informational)?Version)\\s*\\(\\s*\"([0-9.]*)\"\\s*\\)", "$1(\"{0}\")"), version));
                        CmdLine.CommandLine.WriteLineColor(ConsoleColor.DarkCyan, "v{0} [Was v{1}]", version, diversion.New.Version);
                    }
                else if (version == diversion.New.Version)
                    CmdLine.CommandLine.WriteLineColor(ConsoleColor.Green, "v{0}", version);
                else
                    CmdLine.CommandLine.WriteLineColor(ConsoleColor.DarkGreen, "v{0} [Determined v{1}]", diversion.New.Version, version);
            }
            catch (Exception e)
            {
                CmdLine.CommandLine.WriteLineColor(ConsoleColor.Red, e.Message);
            }
        }

        private static void BuildTarget(Options options)
        {
            Console.WriteLine("Building the target project...");
            options.Target = BuildProject(options.Target, options.BuildProperties);
        }

        private static string BuildProject(string projectFile, string properties)
        {
            ProjectCollection projects = new ProjectCollection(ToDictionary(properties));
            var project = projects.LoadProject(projectFile);
            project.Build();
            return Path.Combine(project.DirectoryPath, project.GetPropertyValue("OutputPath"), project.GetPropertyValue("TargetFileName"));
        }

        private static IDictionary<string, string> ToDictionary(string properties)
        {
            return properties.Split(';')
                .Where(property => property.Contains("="))
                .Select(property => property.Split('='))
                .ToDictionary(property => property[0].Trim(), property => property[1].Trim());
        }

        private static string GetProjectFilePath(string path)
        {
            string[] projectExtensions = new[] { ".csproj", ".vbproj", ".proj" };
            var projectFile = Directory.EnumerateFiles(path).Select(file => new FileInfo(file)).FirstOrDefault(file => file.Name.Equals(file.Directory.Name) && projectExtensions.Contains(file.Extension)) ??
                Directory.EnumerateFiles(path).Select(file => new FileInfo(file)).FirstOrDefault(file => projectExtensions.Contains(file.Extension));
            return projectFile == null ? null : projectFile.FullName;
        }


        private static bool GetReleaseUsingGit(Options options)
        {
            Console.WriteLine("Attempting to rebuild the last deployed release from git repository...");
            try
            {
                CloneRemoteReleaseBranch(options);
                string projectFile = GetProjectFilePath(Path.Combine(options.WorkingDirectory, "repo", Path.Combine(options.ProjectDirectory.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Skip(GetLocalGitRepository(options.GitPath).Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Length).ToArray())));
                options.ReleasedPath = BuildProject(projectFile, options.BuildProperties);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool GetReleaseUsingNuGet(Options options)
        {
            if (!Directory.Exists(options.WorkingDirectory))
                Directory.CreateDirectory(options.WorkingDirectory);
            options.NuGetPackageId = options.NuGetPackageId ?? Path.GetFileNameWithoutExtension(options.Target);

            if (!IsNuGetAvailable(options.WorkingDirectory))
                return false;

            Console.WriteLine("Attempting to download the last deployed release from nuget...");

            string packages = Path.Combine(options.WorkingDirectory, "packages");
            string references = Path.Combine(options.WorkingDirectory, "references");
            string nuget = Path.Combine(options.WorkingDirectory, "nuget.exe");
            if (!Directory.Exists(packages))
                Directory.CreateDirectory(packages);
            if (!Directory.Exists(references))
                Directory.CreateDirectory(references);
            var argBuilder = new StringBuilder();
            argBuilder.AppendFormat("install -OutputDirectory \"{0}\" ", packages);
            argBuilder.AppendFormat("{0} {1}", options.NuGetPackageId, options.NuGetPackageVersion);
            ExecuteCommand(nuget, argBuilder.ToString());

            foreach (string assembly in Directory.EnumerateDirectories(packages).Select(path => Path.Combine(path, "lib")).Where(Directory.Exists).SelectMany(path => Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories).Concat(Directory.EnumerateFiles(path, "*.exe", SearchOption.AllDirectories))))
                File.Copy(assembly, Path.Combine(references, Path.GetFileName(assembly)), true);
            if (File.Exists(Path.Combine(references, Path.GetFileName(options.Target))))
                options.ReleasedPath = Path.Combine(references, Path.GetFileName(options.Target));
            return options.ReleasedPath != null;
        }

        private static bool ExecuteCommand(string cmd, string args)
        {
            string output;
            string error;
            return ExecuteCommand(cmd, args, out output, out error);
        }

        private static bool ExecuteCommand(string cmd, string args, out string output, out string error)
        {
            var process = Process.Start(new ProcessStartInfo
            {
                UseShellExecute = false,
                FileName = cmd,
                Arguments = args,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            });
            process.WaitForExit();
            output = process.StandardOutput.ReadToEnd();
            error = process.StandardError.ReadToEnd();
            return process.ExitCode == 0;
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


        private static void CloneRemoteReleaseBranch(Options options)
        {
            if (Directory.Exists(Path.Combine(options.WorkingDirectory, "repo")))
                DeleteReadOnlyDirectory(Path.Combine(options.WorkingDirectory, "repo"));
            options.GitRepository = options.GitReleaseBranch.Contains("/") ? GetGitRepositoryFromRemote(options.GitPath, options.GitReleaseBranch.Substring(0, options.GitReleaseBranch.IndexOf("/"))) : GetLocalGitRepository(options.GitPath);
            options.GitReleaseBranch = options.GitReleaseBranch.Substring(options.GitReleaseBranch.IndexOf("/") + 1);
            ExecuteCommand(options.GitPath, string.Format("clone -b {0} --depth 1 --single-branch {1} \".diversion/repo\"", options.GitReleaseBranch, options.GitRepository));
        }

        private static void DeleteReadOnlyDirectory(string directory)
        {
            foreach (var subdirectory in Directory.EnumerateDirectories(directory))
            {
                DeleteReadOnlyDirectory(subdirectory);
            }
            foreach (var fileName in Directory.EnumerateFiles(directory))
            {
                var fileInfo = new FileInfo(fileName);
                fileInfo.Attributes = FileAttributes.Normal;
                fileInfo.Delete();
            }
            Directory.Delete(directory);
        }

        private static string GetGitRepositoryFromRemote(string git, string remote)
        {
            string output;
            string error;
            if (ExecuteCommand(git, string.Format("remote show -n {0}", remote), out output, out error))
                return Regex.Match(output, "Fetch URL: (.*)").Groups[1].Value;
            throw new ApplicationException(error);
        }

        private static string GetLocalGitRepository(string git)
        {
            string output;
            string error;
            if (ExecuteCommand(git, "rev-parse --show-toplevel", out output, out error))
                return output + ".git";
            throw new ApplicationException(error);
        }
    }
}
