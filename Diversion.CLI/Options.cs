using System;

namespace Diversion.CLI
{
    [CmdLine.CommandLineArguments(Program = "diver.exe", Title = "Diversion.CLI", Description= "A tool for assigning the correct semantic version of an assembly based on its diversion from the last deployed release.")] 
    class Options
    {
        [CmdLine.CommandLineParameter(Command = "?", Name = "Help", Description = "Show Help", Default = false, IsHelp = true)]
        public bool Help { get; set; }

        [CmdLine.CommandLineParameter(Name = "Target", ParameterIndex = 1, Description = "Either the project file or assembly for the proposed version.")]
        public string Target { get; set; }

        [CmdLine.CommandLineParameter(Command = "p", Name = "BuildProperties", Description = "Properties that will be passed to msbuild.", Default = "Configuration=Release")]
        public string BuildProperties { get; set; }

        [CmdLine.CommandLineParameter(Command = "c", Name = "Correct", Description = "Update the assembly versions in AssemblyInfo file to the calculated semantic version.", Default = false)]
        public bool Correct { get; set; }

        [CmdLine.CommandLineParameter(Command = "a", Name = "AssemblyInfoFile", Description = "The path to the file that contains the assembly version attributes.", Default = "Properties\\AssemblyInfo.cs")]
        public string AssemblyInfoFile { get; set; }

        [CmdLine.CommandLineParameter(Command = "s", Name = "ReleaseSource", Description = "The method for retrieving the released version of the assembly. Choices are Auto | NuGet | Git.", Default = CLI.ReleaseSource.Auto)]
        public string ReleaseSource { get; set; }

        [CmdLine.CommandLineParameter(Command = "r", Name = "ReleasedPath", Description = "The path to the released assembly, set this in lieu of using nuget or git to retrieve the last deployed release.")]
        public string ReleasedPath { get; set; }

        [CmdLine.CommandLineParameter(Command = "GitReleaseBranch", Name = "GitReleaseBranch", Description = "The git branch from which to rebuild the released assembly.", Default = "origin/master")]
        public string GitReleaseBranch { get; set; }

        [CmdLine.CommandLineParameter(Command = "GitRepository", Name = "GitRepository", Description = "The git repository to use when rebuilding the released assembly.")]
        public string GitRepository { get; internal set; }

        [CmdLine.CommandLineParameter(Command = "GitPath", Name = "GitPath", Default = "git", Description = "The path to the git executable.")]
        public string GitPath { get; set; }

        [CmdLine.CommandLineParameter(Command = "NuGetPackageId", Name = "NuGetPackageId", Description = "The nuget package id of the package that contains the released assembly.")]
        public string NuGetPackageId { get; set; }

        [CmdLine.CommandLineParameter(Command = "NuGetPackageVersion", Name = "NuGetPackageVersion", Description = "The nuget package version of the package that contains the released assembly.")]
        public string NuGetPackageVersion { get; set; }

        [CmdLine.CommandLineParameter(Command = "Verbosity", Name = "Verbosity", Description = "Amount of detail to output. Choices are Silent | Minimal | Normal | Verbose. ", Default = "Normal")]
        public string Verbosity { get; set; }

        internal Verbosity VerbosityLevel { get { return (Verbosity) Enum.Parse(typeof (Verbosity), Verbosity); } }

        internal string ProjectDirectory { get; set; }

        internal string WorkingDirectory { get; set; }
    }
}