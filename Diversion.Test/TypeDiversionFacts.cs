using Diversion.Reflection;
using Xunit;
using Should.Fluent;

namespace Diversion.Test
{

    public class TypeDiversionFacts
    {
        private IReflectionInfoFactory _factory;

        public TypeDiversionFacts()
        {
            _factory = new NvReflectionInfoFactory();
        }

        [Fact]
        public void OldShouldBeSetProperly()
        {
            var old = new NvTypeInfo(_factory, typeof(object));
            var @new = new NvTypeInfo(_factory, typeof(object));
            var ad = new TypeDiversion(new DiversionDiviner(), old, @new);
            ad.Old.Should().Equal(old);
        }

        [Fact]
        public void NewShouldBeSetProperly()
        {
            var old = new NvTypeInfo(_factory, typeof(object));
            var @new = new NvTypeInfo(_factory, typeof(object));
            var ad = new TypeDiversion(new DiversionDiviner(), old, @new);
            ad.New.Should().Equal(@new);
        }

        [Fact]
        public void InterfaceDiversionsShouldBeSetProperly()
        {
            var old = new NvTypeInfo(_factory, typeof(object));
            var @new = new NvTypeInfo(_factory, typeof(object));
            var ad = new TypeDiversion(new DiversionDiviner(), old, @new);
            ad.InterfaceDiversions.Should().Not.Be.Null();
        }

        [Fact]
        public void MemberDiversionsShouldBeSetProperly()
        {
            var old = new NvTypeInfo(_factory, typeof(object));
            var @new = new NvTypeInfo(_factory, typeof(object));
            var ad = new TypeDiversion(new DiversionDiviner(), old, @new);
            ad.MemberDiversions.Should().Not.Be.Null();
        }

        [Fact]
        public void AttributeDiversionsShouldBeSetProperly()
        {
            var old = new NvTypeInfo(_factory, typeof(object));
            var @new = new NvTypeInfo(_factory, typeof(object));
            var ad = new TypeDiversion(new DiversionDiviner(), old, @new);
            ad.AttributeDiversions.Should().Not.Be.Null();
        }
    }
}
