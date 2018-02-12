using System;
using Xunit;
using Should;

namespace Diversion.Test
{
    
    public class VersionExtensionsFacts
    {
        [Fact]
        public void IncrementMajorTest()
        {
            var version = new Version(1,1,1);
            version.IncrementMajor().ShouldEqual(new Version(2, 0, 0));
            version.ShouldEqual(new Version(1, 1, 1));
        }

        [Fact]
        public void IncrementMinorTest()
        {
            var version = new Version(1, 1, 1);
            version.IncrementMinor().ShouldEqual(new Version(1, 2, 0));
            version.ShouldEqual(new Version(1, 1, 1));
        }

        [Fact]
        public void IncrementMinorWhenNoBuildSpecifiedTest()
        {
            var version = new Version(1, 1);
            version.IncrementMinor().ShouldEqual(new Version(1, 2));
            version.ShouldEqual(new Version(1, 1));
        }

        [Fact]
        public void IncrementBuildTest()
        {
            var version = new Version(1, 1, 1);
            version.IncrementBuild().ShouldEqual(new Version(1, 1, 2));
            version.ShouldEqual(new Version(1, 1, 1));
        }

        [Fact]
        public void IncrementBuildWhenNoBuildSpecifiedTest()
        {
            var version = new Version(1, 1);
            version.IncrementBuild().ShouldEqual(new Version(1, 1, 1));
            version.ShouldEqual(new Version(1, 1));
        }
    }
}
