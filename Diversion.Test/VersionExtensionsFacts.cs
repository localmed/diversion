using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Diversion.Test
{
    [TestClass]
    public class VersionExtensionsFacts
    {
        [TestMethod]
        public void IncrementMajorTest()
        {
            var version = new Version(1,1,1);
            version.IncrementMajor().ShouldEqual(new Version(2, 0, 0));
            version.ShouldEqual(new Version(1, 1, 1));
        }

        [TestMethod]
        public void IncrementMinorTest()
        {
            var version = new Version(1, 1, 1);
            version.IncrementMinor().ShouldEqual(new Version(1, 2, 0));
            version.ShouldEqual(new Version(1, 1, 1));
        }

        [TestMethod]
        public void IncrementMinorWhenNoBuildSpecifiedTest()
        {
            var version = new Version(1, 1);
            version.IncrementMinor().ShouldEqual(new Version(1, 2));
            version.ShouldEqual(new Version(1, 1));
        }

        [TestMethod]
        public void IncrementBuildTest()
        {
            var version = new Version(1, 1, 1);
            version.IncrementBuild().ShouldEqual(new Version(1, 1, 2));
            version.ShouldEqual(new Version(1, 1, 1));
        }

        [TestMethod]
        public void IncrementBuildWhenNoBuildSpecifiedTest()
        {
            var version = new Version(1, 1);
            version.IncrementBuild().ShouldEqual(new Version(1, 1, 1));
            version.ShouldEqual(new Version(1, 1));
        }
    }
}
