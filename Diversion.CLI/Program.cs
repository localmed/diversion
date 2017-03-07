using System;
using Microsoft.Build.Evaluation;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Text;
using Microsoft.Build.Logging;
using Microsoft.Build.Framework;

namespace Diversion.CLI
{
    class Program
    {
        private static readonly string[] SupportedProjectTypes = new[] { ".csproj", ".vbproj", ".proj" };
        static int Main(string[] args)
        {
            int exitCode = -1;
            try
            {
                var options = CmdLine.CommandLine.Parse<Options>();
                if (options.Target == null)
                {
                    Console.WriteLine(new CmdLine.CommandArgumentHelp(typeof(Options)).GetHelpText(Console.BufferWidth).Replace('/', '-'));
                    return exitCode;
                }
                var target = Path.GetFileName(options.Target);
                if (Directory.Exists(options.Target))
                    options.Target = FindProjectFilePath(options.Target);
                if (options.Target == null || !File.Exists(options.Target))
                {
                    WriteLine(options, Verbosity.Silent, ConsoleColor.Red, "A valid target could not be found for {0}.", target);
                    return exitCode;
                }
                bool isProjectFile = !Regex.IsMatch(options.Target, ".*(.dll|.exe)$");
                options.ProjectDirectory = Path.GetDirectoryName(isProjectFile ? options.Target : FindProjectFilePath(Path.GetFileNameWithoutExtension(options.Target)));
                options.WorkingDirectory = Path.Combine(Environment.CurrentDirectory, ".diversion");
                options.AssemblyInfoFile = Path.Combine(options.ProjectDirectory, options.AssemblyInfoFile);
                if (isProjectFile)
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
                    exitCode = DivineDiversion(options);
                else
                    WriteLine(options, Verbosity.Silent, ConsoleColor.Red, "Could not acquire the latest release.");
            }
            catch(CmdLine.CommandLineHelpException clhe)
            {
                Console.WriteLine(clhe.ArgumentHelp.GetHelpText(Console.BufferWidth).Replace('/', '-'));
            }
            catch (CmdLine.CommandLineException exception)
            {
                Console.WriteLine(exception.ArgumentHelp.Message);
                Console.WriteLine(exception.ArgumentHelp.GetHelpText(Console.BufferWidth).Replace('/', '-'));
            }

#if DEBUG
            CmdLine.CommandLine.Pause();
#endif
            return exitCode;
        }

        private static int DivineDiversion(Options options)
        {
            try
            {
                WriteLine(options, Verbosity.Normal, ConsoleColor.White, "Divining the semantic version based on the diversion from the latest release...");
                var diversion = new AssemblyDiversionDiviner().Divine(options.ReleasedPath, options.Target);
                var version = new NextVersion().Determine(diversion);
                if (version == diversion.Old.Version)
                {
                    WriteLine(options, Verbosity.Minimal, ConsoleColor.White, "The target project has not diverged from the released version.");
                    return 1;
                }
                else if (version > diversion.New.Version)
                {
                    if (!options.Correct)
                    {
                        WriteLine(options, Verbosity.Minimal, ConsoleColor.DarkRed, "{0} {1} [Currently {2}]", Path.GetFileNameWithoutExtension(options.Target), version, diversion.New.Version);
                        return 2;
                    }
                    else
                    {
                        WriteLine(options, Verbosity.Normal, ConsoleColor.White, "Updating the assembly info in the target project...");
                        File.WriteAllText(options.AssemblyInfoFile, string.Format(Regex.Replace(File.ReadAllText(options.AssemblyInfoFile), "(Assembly(?:File|Informational)?Version)\\s*\\(\\s*\"([0-9.]*)\"\\s*\\)", "$1(\"{0}\")"), version), Encoding.UTF8);
                        WriteLine(options, Verbosity.Minimal, ConsoleColor.DarkCyan, "{0} {1} [Was {2}]", Path.GetFileNameWithoutExtension(options.Target), version, diversion.New.Version);
                    }
                }
                else if (version == diversion.New.Version)
                {
                    WriteLine(options, Verbosity.Minimal, ConsoleColor.Green, "{0} {1}", Path.GetFileNameWithoutExtension(options.Target), version);
                }
                else
                {
                    WriteLine(options, Verbosity.Minimal, ConsoleColor.DarkGreen, "{0} {1} [Determined {2}]", Path.GetFileNameWithoutExtension(options.Target), diversion.New.Version, version);
                }
                return 0;
            }
            catch (Exception e)
            {
                WriteLine(options, Verbosity.Silent, ConsoleColor.Red, e.Message);
                return -1;
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
            options.Target = BuildProject(options, options.Target);
        }

        private static string BuildProject(Options options, string projectFile)
        {
            ProjectCollection projects = new ProjectCollection(ToDictionary(options.BuildProperties));
            var project = projects.LoadProject(projectFile);
            switch (options.Verbosity)
            {
                case Verbosity.Detailed:
                    project.Build(new ConsoleLogger(LoggerVerbosity.Normal));
                    break;
                case Verbosity.Normal:
                    project.Build(new ConsoleLogger(LoggerVerbosity.Quiet));
                    break;
                default:
                    project.Build();
                    break;
            }
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
                options.ReleasedPath = BuildProject(options, projectFile);
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

            if (!IsNuGetAvailable(options))
                return false;

            options.NuGetPackageId = options.NuGetPackageId ?? Path.GetFileNameWithoutExtension(options.Target);
            WriteLine(options, Verbosity.Normal, ConsoleColor.White, "Attempting to download the latest release from nuget...");
            string nugetWorking = Path.Combine(options.WorkingDirectory, options.NuGetPackageId);
            string packages = Path.Combine(nugetWorking, "packages");
            string references = Path.Combine(nugetWorking, "references");
            if (!Directory.Exists(nugetWorking))
                Directory.CreateDirectory(nugetWorking);
            if (!Directory.Exists(packages))
                Directory.CreateDirectory(packages);
            if (!Directory.Exists(references))
                Directory.CreateDirectory(references);
            var argBuilder = new StringBuilder();
            argBuilder.AppendFormat($"install -OutputDirectory \"{packages}\" ");
            if (!string.IsNullOrWhiteSpace(options.NuGetPackageSource))
                argBuilder.AppendFormat($"-Source {options.NuGetPackageSource} ");
            argBuilder.AppendFormat($"{options.NuGetPackageId} {options.NuGetPackageVersion}");

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

        private static bool ExecuteCommand(string cmd, string args, bool echo = false)
        {
            string output;
            string error;
            return ExecuteCommand(cmd, args, out output, out error);
        }

        private static bool ExecuteCommand(string cmd, string args, out string output, out string error, bool echo = false)
        {
            if (echo) CmdLine.CommandLine.WriteLineColor(ConsoleColor.Cyan, $"{cmd} {args}");
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
            var futureError = process.StandardError.ReadToEndAsync();
            output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            error = futureError.Result;
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
            ExecuteCommand("git", string.Format("clone -b {0} -q --single-branch --reference \"{1}\" \"{2}\" \"{3}\"", options.GitReleaseBranch, GetLocalGitRepositoryPath(), options.GitRepository, Path.Combine(options.WorkingDirectory, "repo")));
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
                return Regex.Match(output, "Fetch URL: (.*)").Groups[1].Value.Trim();
            throw new ApplicationException(error);
        }

        private static string GetLocalGitRepositoryPath()
        {
            string output;
            string error;
            if (ExecuteCommand("git", "rev-parse --show-toplevel", out output, out error))
                return output.Trim();
            throw new ApplicationException(error);
        }

        private static string GetLocalGitRepository()
        {
            return GetLocalGitRepositoryPath() + ".git";
        }
    }
}
