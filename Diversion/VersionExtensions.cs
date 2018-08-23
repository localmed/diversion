using NuGet.Versioning;
using System;

namespace Diversion
{
    public static class VersionExtensions
    {
        public static Version IncrementMajor(this Version it)
        {
            return new Version(it.Major + 1, 0, 0);
        }

        public static Version IncrementMinor(this Version it)
        {
            return it.Build == -1 ? new Version(it.Major, it.Minor + 1) : new Version(it.Major, it.Minor + 1, 0);
        }

        public static Version IncrementBuild(this Version it)
        {
            return new Version(it.Major, it.Minor, it.Build >= 0 ? it.Build + 1 : 1);
        }

        public static NuGetVersion IncrementMajor(this NuGetVersion it)
        {
            return new NuGetVersion(it.Major + 1, 0, 0, it.Release, it.Metadata);
        }

        public static NuGetVersion IncrementMinor(this NuGetVersion it)
        {
            return new NuGetVersion(it.Major, it.Minor + 1, 0, it.Release, it.Metadata);
        }

        public static NuGetVersion IncrementPatch(this NuGetVersion it)
        {
            return new NuGetVersion(it.Major, it.Minor, it.Patch >= 0 ? it.Patch + 1 : 1, it.Release, it.Metadata);
        }

        public static NuGetVersion ToSemanticVersion(this Version it)
        {
            return new NuGetVersion(it.Major, it.Minor, it.Build, it.Revision);
        }
    }
}