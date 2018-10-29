using System;
using Xunit;
using Shouldly;

namespace Diversion.Test
{

    public class VersionExtensionsFixture
    {
        [Fact]
        public void IncrementMajorTest()
        {
            var version = new Version(1,1,1);
            version.IncrementMajor().ShouldBe(new Version(2, 0, 0));
            version.ShouldBe(new Version(1, 1, 1));
        }

        [Fact]
        public void IncrementMinorTest()
        {
            var version = new Version(1, 1, 1);
            version.IncrementMinor().ShouldBe(new Version(1, 2, 0));
            version.ShouldBe(new Version(1, 1, 1));
        }

        [Fact]
        public void IncrementMinorWhenNoBuildSpecifiedTest()
        {
            var version = new Version(1, 1);
            version.IncrementMinor().ShouldBe(new Version(1, 2));
            version.ShouldBe(new Version(1, 1));
        }

        [Fact]
        public void IncrementBuildTest()
        {
            var version = new Version(1, 1, 1);
            version.IncrementBuild().ShouldBe(new Version(1, 1, 2));
            version.ShouldBe(new Version(1, 1, 1));
        }

        [Fact]
        public void IncrementBuildWhenNoBuildSpecifiedTest()
        {
            var version = new Version(1, 1);
            version.IncrementBuild().ShouldBe(new Version(1, 1, 1));
            version.ShouldBe(new Version(1, 1));
        }
    }
}
