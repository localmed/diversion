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
    }
}