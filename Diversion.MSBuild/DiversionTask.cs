using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Diversion.Reflection;

namespace Diversion.MSBuild
{
    public class DiversionTask : Task
    {
        [Output]
        public bool Verified { get; set; }
        [Output]
        public string Version { get; set; }
        [Output]
        public bool HasDiverged { get; set; }
        [Output]
        public bool RequiresCorrection { get; set; }

        public string PackageId { get; set; }
        public string TargetPath { get; set; }
        public string TargetFramework { get; set; }
        public string TargetFrameworkVersion { get; set; }
        public string ReleasedOutputPath { get; set; }
        public string ConfigFilePath { get; set; }

        public override bool Execute()
        {
            Configuration = GetConfiguration();
            if (string.IsNullOrWhiteSpace(TargetFramework))
                TargetFramework = TargetFrameworkVersion.Replace("v", "net").Replace(".", "");
            if (string.IsNullOrWhiteSpace(PackageId))
                PackageId = Path.GetFileNameWithoutExtension(TargetPath);
            var releasePath = GetLatestReleasePath();
            if (string.IsNullOrWhiteSpace(releasePath) || !File.Exists(releasePath))
            {
                Log.LogMessage(MessageImportance.High, $"A release could not be located for {PackageId}; the assumption is that this will be the first published version for this framework.");
                HasDiverged = true;
                return true;
            }
            var diversion = new AssemblyDiversionDiviner(new NvAssemblyInfoFactory(), new DiversionDiviner()).Divine(releasePath, TargetPath);
            if (Configuration.GenerateDiversionFile)
            {
                using (var writer = new StreamWriter(File.Create(Path.Combine(Path.GetDirectoryName(TargetPath), "diversion.output.json"))))
                    JsonSerializer.CreateDefault(new JsonSerializerSettings { ContractResolver = new CustomContractResolver() }).Serialize(writer, diversion);
                Log.LogMessage(MessageImportance.High, "Generated diversion.output.json.");
            }
            var nextVersion = new NextVersion().Determine(diversion);
            var version = diversion.New.Version >= nextVersion ? diversion.New.Version : nextVersion;
            HasDiverged = diversion.HasDiverged();
            Version = version.ToString();
            Verified = version == diversion.New.Version;
            RequiresCorrection = !Verified && Configuration.IsCorrectionEnabled;
            if (!HasDiverged)
                Log.LogMessage(MessageImportance.High, $"{diversion.Identity} has not diverged from its latest release.");
            if (!Verified && !Configuration.IsCorrectionEnabled)
            {
                var message = $"Based on diversion's semantic version rules, the version of {diversion.Identity} being built should be at least {nextVersion}, but is set at {diversion.New.Version}.";
                if (Configuration.Warn)
                    Log.LogWarning(message);
                else
                    Log.LogError(message);
            }
            if (Verified && HasDiverged)
                Log.LogMessage(MessageImportance.High, $"Based on diversion's semantic version rules, the version of {diversion.Identity} being built has the correct version of {diversion.New.Version}.");
            if (RequiresCorrection)
                Log.LogMessage(MessageImportance.High, $"Based on diversion's semantic version rules, the version of {diversion.Identity} will be changed from {diversion.Old.Version} to {nextVersion}.");
            return Configuration.IsCorrectionEnabled || Verified || Configuration.Warn;
        }

        private DiversionConfiguration GetConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new Dictionary<string, string>{
                { "IsCorrectionEnabled", "false"},
                { "GenerateDiversionFile", "false"},
                { "Warn", "false" }
            });
            if (File.Exists(ConfigFilePath))
                builder = builder.AddJsonFile(ConfigFilePath);
            return builder.Build().Get<DiversionConfiguration>();
        }

        private string GetLatestReleasePath()
        {
            var projectFile = new FileInfo(Path.Combine(ReleasedOutputPath, "latest", "project.csproj"));
            if (!projectFile.Directory.Exists)
                projectFile.Directory.Create();
            using (var stream = projectFile.CreateText())
                stream.WriteLine($"<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>{TargetFramework}</TargetFramework></PropertyGroup><ItemGroup><PackageReference Include=\"{PackageId}\" Version=\"*\" /></ItemGroup></Project>");
            Log.LogMessage(MessageImportance.High, $"Attempting to acquire latest released version of {PackageId} for {TargetFramework}.");
            if (BuildEngine.BuildProjectFile(projectFile.FullName, new[] { "Restore", "Build" }, new Dictionary<string, string> { { "Configuration", "Release" } } , null))
                return Path.Combine(projectFile.DirectoryName, "bin", "Release", TargetFramework, Path.GetFileName(TargetPath));
            return string.Empty;
        }

        public DiversionConfiguration Configuration { get; set; }

        private class CustomContractResolver : DefaultContractResolver
        {
            protected override List<MemberInfo> GetSerializableMembers(Type objectType)
            {
                var members = base.GetSerializableMembers(objectType);
                return typeof(IAssemblyDiversion).IsAssignableFrom(objectType) ? members.FindAll(m => !m.Name.Equals("Old") && !m.Name.Equals("New")) : members;
            }
        }
    }

    public class DiversionConfiguration
    {
        public bool IsCorrectionEnabled { get; set; }
        public bool GenerateDiversionFile { get; set; }
        public bool Warn { get; set; }
    }
}
