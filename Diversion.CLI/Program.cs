using System;
using Microsoft.Build.Evaluation;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Diversion.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = CmdLine.CommandLine.Parse<Options>();
            options.ProjectFile = options.ProjectFile ?? GetProjectFilePath();
            options.ReleasedPath = options.ReleasedPath ?? GetReleasedPath(options.ProjectFile);
            options.ProposedPath = options.ProposedPath ?? GetProposedPath(options.ProjectFile);
            var diversion = new AssemblyDiversionDiviner().Divine(options.ReleasedPath, options.ProposedPath);
            var version = new NextVersion().Determine(diversion);
            if (version > diversion.New.Version)
                if (!options.Correct)
                {
                    CmdLine.CommandLine.WriteLineColor(ConsoleColor.DarkRed, "v{0} [Currently v{1}]", version, diversion.New.Version);
                }
                else
                {
                    CmdLine.CommandLine.WriteLineColor(ConsoleColor.DarkCyan, "v{0} [Was v{1}]", version, diversion.New.Version);
                }
            else if (version == diversion.New.Version)
                CmdLine.CommandLine.WriteLineColor(ConsoleColor.Green, "v{0}", version);
            else
                CmdLine.CommandLine.WriteLineColor(ConsoleColor.DarkGreen, "v{0} [Determined v{1}]", diversion.New.Version, version);
            Console.ReadKey();
        }

        private static string GetProposedPath(string projectFile)
        {
            ProjectCollection projects = new ProjectCollection(new Dictionary<string, string> {{ "Configuration", "Release" }});
            var project = projects.LoadProject(projectFile);
            project.Build();
            return Path.Combine(project.DirectoryPath, project.GetPropertyValue("OutputPath"), project.GetPropertyValue("TargetFileName"));
        }

        private static string GetReleasedPath(string projectFile)
        {                        
            ProjectCollection projects = new ProjectCollection(new Dictionary<string, string> { { "Configuration", "Release" } });
            var project = projects.LoadProject(projectFile);
            project.Build();
            return Path.Combine(project.DirectoryPath, project.GetPropertyValue("OutputPath"), project.GetPropertyValue("TargetFileName"));
        }

        private static string GetProjectFilePath()
        {
            string[] projectExtensions = new[] { ".csproj", ".vbproj", ".proj" };
            var projectFile = Directory.EnumerateFiles(Environment.CurrentDirectory).Select(file => new FileInfo(file)).FirstOrDefault(file => file.Name.Equals(file.Directory.Name) && projectExtensions.Contains(file.Extension)) ??
                Directory.EnumerateFiles(Environment.CurrentDirectory).Select(file => new FileInfo(file)).FirstOrDefault(file => projectExtensions.Contains(file.Extension));
            return projectFile == null ? null : projectFile.FullName;
        }


    }

    class Options
    {
        [CmdLine.CommandLineParameter(Name = "ProposedPath", ParameterIndex = 1)]
        public string ProposedPath { get; set; }

        [CmdLine.CommandLineParameter("r", Name = "ReleasedPath")]
        public string ReleasedPath { get; set; }

        [CmdLine.CommandLineParameter("f", Name = "ProjectFile")]
        public string ProjectFile { get; set; }

        [CmdLine.CommandLineParameter("c", Name = "Correct")]
        public bool Correct { get; set; }

        [CmdLine.CommandLineParameter("a", Name = "AssemblyInfoFile", Default = "Properties\\AssemblyInfo.cs")]
        public string AssemblyInfoFile { get; set; }

        [CmdLine.CommandLineParameter("s", Name = "ReleaseSource", Default = ReleaseSource.Auto)]
        public ReleaseSource ReleaseSource { get; set; }
    }

    enum ReleaseSource
    {
        Auto,
        NuGet,
        Git,
        File        
    }
}
