using System.Reflection;
using Diversion.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

namespace Diversion.Test
{
    [TestClass]
    public class AssemblyDiversionFacts
    {
        [TestMethod]
        public void OldShouldBeSetProperly()
        {
            var old = new NvAssemblyInfo(Assembly.GetExecutingAssembly().Location);
            var @new = new NvAssemblyInfo(Assembly.GetExecutingAssembly().Location);
            var ad = new AssemblyDiversion(new DiversionDiviner(), old, @new);
            ad.Old.Should().Equal(old);
        }

        [TestMethod]
        public void NewShouldBeSetProperly()
        {
            var old = new NvAssemblyInfo(Assembly.GetExecutingAssembly().Location);
            var @new = new NvAssemblyInfo(Assembly.GetExecutingAssembly().Location);
            var ad = new AssemblyDiversion(new DiversionDiviner(), old, @new);
            ad.New.Should().Equal(@new);
        }

        [TestMethod]
        public void TypeDiversionsShouldBeSetProperly()
        {
            var old = new NvAssemblyInfo(Assembly.GetExecutingAssembly().Location);
            var @new = new NvAssemblyInfo(Assembly.GetExecutingAssembly().Location);
            var ad = new AssemblyDiversion(new DiversionDiviner(), old, @new);
            ad.TypeDiversions.Should().Not.Be.Null();
        }

        [TestMethod]
        public void AttributeDiversionsShouldBeSetProperly()
        {
            var old = new NvAssemblyInfo(Assembly.GetExecutingAssembly().Location);
            var @new = new NvAssemblyInfo(Assembly.GetExecutingAssembly().Location);
            var ad = new AssemblyDiversion(new DiversionDiviner(), old, @new);
            ad.AttributeDiversions.Should().Not.Be.Null();
        }
    }
}
