using NuGet.Frameworks;
using NuGet.Versioning;

namespace Diversion
{
    public class NextVersionAnalysis
    {
        public NextVersionAnalysis(string identity, NuGetFramework targetFramework,
            NuGetVersion oldVersion, NuGetVersion newVersion, NuGetVersion calculatedVersion)
        {
            Identity = identity;
            TargetFramework = targetFramework;
            OldVersion = oldVersion;
            NewVersion = newVersion;
            CalculatedVersion = calculatedVersion;
        }

        public string Identity { get; }
        public NuGetFramework TargetFramework { get; }
        public NuGetVersion OldVersion { get; }
        public NuGetVersion NewVersion { get; }
        public NuGetVersion CalculatedVersion { get; }
        public bool HasDiverged => !Equals(OldVersion, CalculatedVersion);
        public bool IsNewVersionCorrect => NewVersion >= CalculatedVersion;
    }
}
