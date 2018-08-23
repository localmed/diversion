using Microsoft.Build.Framework;
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Configuration;
using NuGet.Versioning;
using NuGet.Frameworks;
using System.Runtime.Versioning;
using System.Diagnostics;

namespace Diversion.MSBuild
{
    public class DiversionTask : Nerdbank.MSBuildExtension.ContextIsolatedTask
    {
        [Output]
        public bool Verified { get; set; }
        [Output]
        public string Version { get; set; }
        [Output]
        public string PackageVersion { get; set; }
        [Output]
        public bool HasDiverged { get; set; }
        [Output]
        public bool RequiresCorrection { get; set; }

        public string PackageId { get; set; }
        public string ExistingVersion { get; set; }
        public string TargetPath { get; set; }
        public string ReleasePath { get; set; }
        public string AnalyzePath { get; set; }
        public string ConfigFilePath { get; set; }

        private NextVersionAnalysis ExecuteAnalysis(string releasedAssemblyPath, string targetAssemblyPath, string diversionFilePath)
        {
            var psi = new ProcessStartInfo
            {
                FileName = AnalyzePath,
                Arguments = $"\"{releasedAssemblyPath}\" \"{targetAssemblyPath}\"{(string.IsNullOrWhiteSpace(diversionFilePath) ? string.Empty : $" \"{diversionFilePath}\"")}",
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            using (var process = Process.Start(psi))
            {
                var output = process.StandardOutput.ReadToEnd();
                return JsonConvert.DeserializeObject<NextVersionAnalysis>(output, new NuGetFrameworkConverter(), new NuGetVersionConverter());
            }
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

        protected override bool ExecuteIsolated()
        {
            Configuration = GetConfiguration();
            if (string.IsNullOrWhiteSpace(ReleasePath) || !File.Exists(ReleasePath))
            {
                Log.LogMessage(MessageImportance.High, $"A release could not be located for {PackageId}; the assumption is that this will be the first published version for this framework.");
                HasDiverged = true;
                return true;
            }
            var analysis = ExecuteAnalysis(ReleasePath, TargetPath, Configuration.GenerateDiversionFile ? Path.Combine(Path.GetDirectoryName(TargetPath), "diversion.output.json") : null);
            HasDiverged = analysis.HasDiverged;
            Verified = analysis.IsNewVersionCorrect;
            var correctNewVersion = analysis.IsNewVersionCorrect ?  analysis.NewVersion : analysis.CalculatedVersion; 
            Version = correctNewVersion.ToFullString();
            PackageVersion = string.IsNullOrEmpty(ExistingVersion) || correctNewVersion > new NuGetVersion(ExistingVersion) ? Version : ExistingVersion;
            RequiresCorrection = !Verified && Configuration.IsCorrectionEnabled;
            if (!HasDiverged)
                Log.LogMessage(MessageImportance.High, $"{analysis.Identity} has not diverged from its latest release.");
            if (!Verified && !Configuration.IsCorrectionEnabled)
            {
                var message = $"Based on diversion's semantic version rules, the version of {analysis.Identity} being built should be at least {analysis.CalculatedVersion}, but is set at {analysis.NewVersion}.";
                if (Configuration.Warn)
                    Log.LogWarning(message);
                else
                    Log.LogError(message);
            }
            if (Verified && HasDiverged)
                Log.LogMessage(MessageImportance.High, $"Based on diversion's semantic version rules, the version of {analysis.Identity} being built has the correct version of {analysis.NewVersion}.");
            if (RequiresCorrection)
                Log.LogMessage(MessageImportance.High, $"Based on diversion's semantic version rules, the version of {analysis.Identity} will be changed from {analysis.OldVersion} to {correctNewVersion}.");
            return Configuration.IsCorrectionEnabled || Verified || Configuration.Warn;
        }

        private DiversionConfiguration Configuration { get; set; }

    }

    class CustomContractResolver : DefaultContractResolver
    {
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            var members = base.GetSerializableMembers(objectType);
            return typeof(IAssemblyDiversion).IsAssignableFrom(objectType) ? members.FindAll(m => !m.Name.Equals("Old") && !m.Name.Equals("New")) : members;
        }
    }
    class NuGetVersionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(NuGetVersion).Equals(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return new NuGetVersion((string)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((NuGetVersion)value).ToFullString());
        }
    }

    class NuGetFrameworkConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(NuGetFramework).Equals(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var frameworkName = new FrameworkName((string)reader.Value);
            return new NuGetFramework(frameworkName.Identifier, frameworkName.Version, frameworkName.Profile);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((NuGetFramework)value).DotNetFrameworkName);
        }
    }


    [Serializable]
    public class DiversionConfiguration
    {
        public bool IsCorrectionEnabled { get; set; }
        public bool GenerateDiversionFile { get; set; }
        public bool Warn { get; set; }
    }
}
