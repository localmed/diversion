using System.Reflection;
using Diversion.Reflection;
using Xunit;
using Shouldly;
using System.IO;
using System;

namespace Diversion.Test
{
    public class AssemblyDiversionFacts
    {
        [Fact]
        public void OldShouldBeSetProperly()
        {
            var factory = new NvAssemblyInfoFactory();
            var old = factory.FromFile(Path.Combine(Environment.CurrentDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location)));
            var @new = factory.FromFile(Path.Combine(Environment.CurrentDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location)));
            var ad = new AssemblyDiversion(new DiversionDiviner(), old, @new);
            ad.Old.ShouldBe(old);
        }

        [Fact]
        public void NewShouldBeSetProperly()
        {
            var factory = new NvAssemblyInfoFactory();
            var old = factory.FromFile(Path.Combine(Environment.CurrentDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location)));
            var @new = factory.FromFile(Path.Combine(Environment.CurrentDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location)));
            var ad = new AssemblyDiversion(new DiversionDiviner(), old, @new);
            ad.New.ShouldBe(@new);
        }

        [Fact]
        public void TypeDiversionsShouldBeSetProperly()
        {
            var factory = new NvAssemblyInfoFactory();
            var old = factory.FromFile(Path.Combine(Environment.CurrentDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location)));
            var @new = factory.FromFile(Path.Combine(Environment.CurrentDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location)));
            var ad = new AssemblyDiversion(new DiversionDiviner(), old, @new);
            ad.TypeDiversions.ShouldNotBeNull();
        }

        [Fact]
        public void AttributeDiversionsShouldBeSetProperly()
        {
            var factory = new NvAssemblyInfoFactory();
            var old = factory.FromFile(Path.Combine(Environment.CurrentDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location)));
            var @new = factory.FromFile(Path.Combine(Environment.CurrentDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location)));
            var ad = new AssemblyDiversion(new DiversionDiviner(), old, @new);
            ad.AttributeDiversions.ShouldNotBeNull();
        }

        [Fact]
        public void TheSameAssemblyCannotHaveDiverged()
        {
            var factory = new NvAssemblyInfoFactory();
            var old = factory.FromFile(Path.Combine(Environment.CurrentDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location)));
            var @new = factory.FromFile(Path.Combine(Environment.CurrentDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location)));
            var ad = new AssemblyDiversion(new DiversionDiviner(), old, @new);
            ad.HasDiverged().ShouldBeFalse();
        }

    }
}
