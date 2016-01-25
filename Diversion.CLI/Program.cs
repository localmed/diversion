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
        private static readonly string[] SupportedProjectTypes = new[] { ".csproj", ".vbproj", ".proj" };
        static void Main(string[] args)
        {
            try
            {
                var options = CmdLine.CommandLine.Parse<Options>();
                options.Target = options.Target ?? Environment.CurrentDirectory;
                var target = Path.GetFileName(options.Target);
                if (Directory.Exists(options.Target))
                    options.Target = FindProjectFilePath(options.Target);
                if (options.Target == null || !File.Exists(options.Target))
                {
                    WriteLine(options, Verbosity.Silent, ConsoleColor.Red, "A valid target could not be found for {0}.", target);
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
                    WriteLine(options, Verbosity.Silent, ConsoleColor.Red, "Could not acquire the latest release.");
            }
            catch (CmdLine.CommandLineException exception)
            {
                Console.WriteLine(exception.ArgumentHelp.Message);
                Console.WriteLine(exception.ArgumentHelp.GetHelpText(Console.BufferWidth).Replace('/', '-'));
            }

#if DEBUG
            CmdLine.CommandLine.Pause();
#endif
        }

        private static void DivineDiversion(Options options)
        {
            try
            {
                WriteLine(options, Verbosity.Normal, ConsoleColor.White, "Divining the semantic version based on the diversion from the latest release...");
                var diversion = new AssemblyDiversionDiviner().Divine(options.ReleasedPath, options.Target);
                var version = new NextVersion().Determine(diversion);
                if (version > diversion.New.Version)
                    if (!options.Correct)
                    {
                        WriteLine(options, Verbosity.Minimal, ConsoleColor.DarkRed, "{0} {1} [Currently {2}]", Path.GetFileNameWithoutExtension(options.Target), version, diversion.New.Version);
                    }
                    else
                    {
                        WriteLine(options, Verbosity.Normal, ConsoleColor.White, "Updating the assembly info in the target project...");
                        File.WriteAllText(options.AssemblyInfoFile, string.Format(Regex.Replace(File.ReadAllText(options.AssemblyInfoFile), "(Assembly(?:File|Informational)?Version)\\s*\\(\\s*\"([0-9.]*)\"\\s*\\)", "$1(\"{0}\")"), version), Encoding.UTF8);
                        WriteLine(options, Verbosity.Minimal, ConsoleColor.DarkCyan, "{0} {1} [Was {2}]", Path.GetFileNameWithoutExtension(options.Target), version, diversion.New.Version);
                    }
                else if (version == diversion.New.Version)
                    WriteLine(options, Verbosity.Minimal, ConsoleColor.Green, "{0} {1}", Path.GetFileNameWithoutExtension(options.Target), version);
                else
                    WriteLine(options, Verbosity.Minimal, ConsoleColor.DarkGreen, "{0} {1} [Determined {2}]", Path.GetFileNameWithoutExtension(options.Target), diversion.New.Version, version);
            }
            catch (Exception e)
            {
                WriteLine(options, Verbosity.Silent, ConsoleColor.Red, e.Message);
            }
        }

        private static void WriteLine(Options options, Verbosity level, ConsoleColor color, string format, params object[] formatArgs)
        {
            if (options.Verbosity >= level)
                CmdLine.CommandLine.WriteLineColor(color, format, formatArgs);
        }

        private static void BuildTarget(Options options)
        {
            WriteLine(options, Verbosity.Normal, ConsoleColor.White, "Building {0}...", Path.GetFileNameWithoutExtension(options.Target));
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

        private static string FindProjectFilePath(string path)
        {
            return FindContainerFilePath(path, SupportedProjectTypes);
        }

        private static string FindContainerFilePath(string path, string[] extensions)
        {
            var projectFile =
                Directory.EnumerateFiles(path)
                    .Select(file => new FileInfo(file))
                    .FirstOrDefault(file => file.Name.Equals(file.Directory.Name) && extensions.Contains(file.Extension)) ??
                Directory.EnumerateFiles(path)
                    .Select(file => new FileInfo(file))
                    .FirstOrDefault(file => extensions.Contains(file.Extension));
            return projectFile == null ? null : projectFile.FullName;
        }

        private static string FindSolutionFilePath(string path)
        {
            return FindContainerFilePath(path, new[] {".sln"});
        }

        private static bool GetReleaseUsingGit(Options options)
        {
            if (!IsCommandAvailable("git.exe"))
            {
                WriteLine(options, Verbosity.Minimal, ConsoleColor.Red,  "git.exe is not available.");
                return false;
            }
            WriteLine(options, Verbosity.Normal, ConsoleColor.White, "Attempting to rebuild the latest release from git repository...");
            try
            {
                CloneRemoteReleaseBranch(options);
                if (IsNuGetAvailable(options))
                {
                    string output, error;
                    ExecuteCommand("nuget", string.Format("restore \"{0}\"",  FindSolutionFilePath(Path.Combine(options.WorkingDirectory, "repo"))), out output, out error);
                    if (!string.IsNullOrWhiteSpace(output))
                        WriteLine(options, Verbosity.Normal, ConsoleColor.White, output);
                    if (!string.IsNullOrWhiteSpace(error))
                        WriteLine(options, Verbosity.Minimal, ConsoleColor.Red, error);
                }
                string projectFile = FindProjectFilePath(Path.Combine(options.WorkingDirectory, "repo", Path.Combine(options.ProjectDirectory.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Skip(GetLocalGitRepository().Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Length).ToArray())));
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

            if (!IsNuGetAvailable(options))
                return false;

            WriteLine(options, Verbosity.Normal, ConsoleColor.White, "Attempting to download the latest release from nuget...");

            string packages = Path.Combine(options.WorkingDirectory, "packages");
            string references = Path.Combine(options.WorkingDirectory, "references");
            if (!Directory.Exists(packages))
                Directory.CreateDirectory(packages);
            if (!Directory.Exists(references))
                Directory.CreateDirectory(references);
            var argBuilder = new StringBuilder();
            argBuilder.AppendFormat("install -OutputDirectory \"{0}\" ", packages);
            argBuilder.AppendFormat("{0} {1}", options.NuGetPackageId, options.NuGetPackageVersion);
            ExecuteCommand("nuget", argBuilder.ToString());

            foreach (string assembly in AssembliesInPackages(packages))
                File.Copy(assembly, Path.Combine(references, Path.GetFileName(assembly)), true);
            if (File.Exists(Path.Combine(references, Path.GetFileName(options.Target))))
                options.ReleasedPath = Path.Combine(references, Path.GetFileName(options.Target));
            return options.ReleasedPath != null;
        }

        private static IEnumerable<string> AssembliesInPackages(string packages)
        {
            return Directory.EnumerateDirectories(packages).Select(path => Path.Combine(path, "lib"))
                .Where(Directory.Exists).SelectMany(
                    path => Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories)
                        .Concat(Directory.EnumerateFiles(path, "*.exe", SearchOption.AllDirectories)));
        }

        private static bool IsCommandAvailable(string cmd)
        {
            return Environment.ExpandEnvironmentVariables(Environment.GetEnvironmentVariable("Path") ?? string.Empty).Split(';')
                .Any(path => File.Exists(Path.Combine(path, cmd)));
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

        private static bool IsNuGetAvailable(Options options)
        {
            if (IsCommandAvailable("nuget.exe"))
                return true;
            string nugetPath = Path.Combine(options.WorkingDirectory, "nuget.exe");
            if (File.Exists(nugetPath))
                return true;
            try
            {
                WriteLine(options, Verbosity.Normal, ConsoleColor.White, "Downloading latest version of nuget.exe...");
                new WebClient().DownloadFile("https://dist.nuget.org/win-x86-commandline/latest/nuget.exe", nugetPath);
                Environment.SetEnvironmentVariable("Path", Environment.GetEnvironmentVariable("Path") + ";" + options.WorkingDirectory);
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
            options.GitRepository = options.GitReleaseBranch.Contains("/") ? GetGitRepositoryFromRemote(options.GitReleaseBranch.Substring(0, options.GitReleaseBranch.IndexOf("/"))) : GetLocalGitRepository();
            options.GitReleaseBranch = options.GitReleaseBranch.Substring(options.GitReleaseBranch.IndexOf("/") + 1);
            ExecuteCommand("git", string.Format("clone -b {0} --depth 1 --single-branch {1} \".diversion/repo\"", options.GitReleaseBranch, options.GitRepository));
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

        private static string GetGitRepositoryFromRemote(string remote)
        {
            string output;
            string error;
            if (ExecuteCommand("git", string.Format("remote show -n {0}", remote), out output, out error))
                return Regex.Match(output, "Fetch URL: (.*)").Groups[1].Value;
            throw new ApplicationException(error);
        }

        private static string GetLocalGitRepository()
        {
            string output;
            string error;
            if (ExecuteCommand("git", "rev-parse --show-toplevel", out output, out error))
                return output + ".git";
            throw new ApplicationException(error);
        }
    }
}
