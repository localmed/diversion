using System;
using System.Diagnostics.Contracts;

namespace Diversion
{
    public static class VersionExtensions
    {
        public static Version IncrementMajor(this Version it)
        {
            Contract.Requires(it != null);
            return new Version(it.Major + 1, 0, 0);
        }

        public static Version IncrementMinor(this Version it)
        {
            Contract.Requires(it != null);
            return it.Build == -1 ? new Version(it.Major, it.Minor + 1) : new Version(it.Major, it.Minor + 1, 0);
        }

        public static Version IncrementBuild(this Version it)
        {
            Contract.Requires(it != null);
            return new Version(it.Major, it.Minor, it.Build >= 0 ? it.Build + 1 : 1);
        }
    }
}