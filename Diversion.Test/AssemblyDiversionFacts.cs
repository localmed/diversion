using System.Reflection;
using Diversion.Reflection;
using Xunit;
using Should.Fluent;

namespace Diversion.Test
{
    public class AssemblyDiversionFacts
    {
        [Fact]
        public void OldShouldBeSetProperly()
        {
            var old = new NvAssemblyInfo(Assembly.GetExecutingAssembly().Location);
            var @new = new NvAssemblyInfo(Assembly.GetExecutingAssembly().Location);
            var ad = new AssemblyDiversion(new DiversionDiviner(), old, @new);
            ad.Old.Should().Equal(old);
        }

        [Fact]
        public void NewShouldBeSetProperly()
        {
            var old = new NvAssemblyInfo(Assembly.GetExecutingAssembly().Location);
            var @new = new NvAssemblyInfo(Assembly.GetExecutingAssembly().Location);
            var ad = new AssemblyDiversion(new DiversionDiviner(), old, @new);
            ad.New.Should().Equal(@new);
        }

        [Fact]
        public void TypeDiversionsShouldBeSetProperly()
        {
            var old = new NvAssemblyInfo(Assembly.GetExecutingAssembly().Location);
            var @new = new NvAssemblyInfo(Assembly.GetExecutingAssembly().Location);
            var ad = new AssemblyDiversion(new DiversionDiviner(), old, @new);
            ad.TypeDiversions.Should().Not.Be.Null();
        }

        [Fact]
        public void AttributeDiversionsShouldBeSetProperly()
        {
            var old = new NvAssemblyInfo(Assembly.GetExecutingAssembly().Location);
            var @new = new NvAssemblyInfo(Assembly.GetExecutingAssembly().Location);
            var ad = new AssemblyDiversion(new DiversionDiviner(), old, @new);
            ad.AttributeDiversions.Should().Not.Be.Null();
        }
    }
}
