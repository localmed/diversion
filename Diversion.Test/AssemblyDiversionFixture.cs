using System.Reflection;
using Diversion.Reflection;
using Xunit;
using Shouldly;
using System.IO;
using System;

namespace Diversion.Test
{
    public class AssemblyDiversionFixture
    {
        [Theory, AutoMoqData]
        public void OldShouldBeSetProperly(IAssemblyInfoFactory sut)
        {
            var old = sut.FromFile(Path.Combine(Environment.CurrentDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location)));
            var @new = sut.FromFile(Path.Combine(Environment.CurrentDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location)));
            var ad = new AssemblyDiversion(new DiversionDiviner(), old, @new);
            ad.Old.ShouldBe(old);
        }

        [Theory, AutoMoqData]
        public void NewShouldBeSetProperly(IAssemblyInfoFactory factory)
        {
            var old = factory.FromFile(Path.Combine(Environment.CurrentDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location)));
            var @new = factory.FromFile(Path.Combine(Environment.CurrentDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location)));
            var ad = new AssemblyDiversion(new DiversionDiviner(), old, @new);
            ad.New.ShouldBe(@new);
        }

        [Theory, AutoMoqData]
        public void TypeDiversionsShouldBeSetProperly(IAssemblyInfoFactory factory)
        {
            var old = factory.FromFile(Path.Combine(Environment.CurrentDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location)));
            var @new = factory.FromFile(Path.Combine(Environment.CurrentDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location)));
            var ad = new AssemblyDiversion(new DiversionDiviner(), old, @new);
            ad.TypeDiversions.ShouldNotBeNull();
        }

        [Theory, AutoMoqData]
        public void AttributeDiversionsShouldBeSetProperly(IAssemblyInfoFactory factory)
        {
            var old = factory.FromFile(Path.Combine(Environment.CurrentDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location)));
            var @new = factory.FromFile(Path.Combine(Environment.CurrentDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location)));
            var ad = new AssemblyDiversion(new DiversionDiviner(), old, @new);
            ad.AttributeDiversions.ShouldNotBeNull();
        }

        [Theory, AutoMoqData]
        public void TheSameAssemblyCannotHaveDiverged(IAssemblyInfoFactory factory)
        {
            var old = factory.FromFile(Path.Combine(Environment.CurrentDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location)));
            var @new = factory.FromFile(Path.Combine(Environment.CurrentDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location)));
            var ad = new AssemblyDiversion(new DiversionDiviner(), old, @new);
            ad.HasDiverged().ShouldBeFalse();
        }

    }
}
