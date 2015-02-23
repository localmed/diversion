using Diversion.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

namespace Diversion.Test
{
    [TestClass]
    public class TypeDiversionFacts
    {
        private IReflectionInfoFactory _factory;

        [TestInitialize]
        public void Setup()
        {
            _factory = new NvReflectionInfoFactory();
        }

        [TestMethod]
        public void OldShouldBeSetProperly()
        {
            var old = new NvTypeInfo(_factory, typeof(object));
            var @new = new NvTypeInfo(_factory, typeof(object));
            var ad = new TypeDiversion(new DiversionDiviner(), old, @new);
            ad.Old.Should().Equal(old);
        }

        [TestMethod]
        public void NewShouldBeSetProperly()
        {
            var old = new NvTypeInfo(_factory, typeof(object));
            var @new = new NvTypeInfo(_factory, typeof(object));
            var ad = new TypeDiversion(new DiversionDiviner(), old, @new);
            ad.New.Should().Equal(@new);
        }

        [TestMethod]
        public void InterfaceDiversionsShouldBeSetProperly()
        {
            var old = new NvTypeInfo(_factory, typeof(object));
            var @new = new NvTypeInfo(_factory, typeof(object));
            var ad = new TypeDiversion(new DiversionDiviner(), old, @new);
            ad.InterfaceDiversions.Should().Not.Be.Null();
        }

        [TestMethod]
        public void MemberDiversionsShouldBeSetProperly()
        {
            var old = new NvTypeInfo(_factory, typeof(object));
            var @new = new NvTypeInfo(_factory, typeof(object));
            var ad = new TypeDiversion(new DiversionDiviner(), old, @new);
            ad.MemberDiversions.Should().Not.Be.Null();
        }

        [TestMethod]
        public void AttributeDiversionsShouldBeSetProperly()
        {
            var old = new NvTypeInfo(_factory, typeof(object));
            var @new = new NvTypeInfo(_factory, typeof(object));
            var ad = new TypeDiversion(new DiversionDiviner(), old, @new);
            ad.AttributeDiversions.Should().Not.Be.Null();
        }
    }
}
