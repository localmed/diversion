using Diversion.Reflection;
using Xunit;
using Shouldly;

namespace Diversion.Test
{

    public class TypeDiversionFixture
    {
        [Theory, AutoMoqData]
        public void OldShouldBeSetProperly(ITypeInfo old, ITypeInfo @new)
        {
            var ad = new TypeDiversion(new DiversionDiviner(), old, @new);
            ad.Old.ShouldBeSameAs(old);
        }

        [Theory, AutoMoqData]
        public void NewShouldBeSetProperly(ITypeInfo old, ITypeInfo @new)
        {
            var ad = new TypeDiversion(new DiversionDiviner(), old, @new);
            ad.New.ShouldBeSameAs(@new);
        }

        [Theory, AutoMoqData]
        public void InterfaceDiversionsShouldBeSetProperly(ITypeInfo old, ITypeInfo @new)
        {
            var ad = new TypeDiversion(new DiversionDiviner(), old, @new);
            ad.InterfaceDiversions.ShouldNotBeNull();
        }

        [Theory, AutoMoqData]
        public void MemberDiversionsShouldBeSetProperly(ITypeInfo old, ITypeInfo @new)
        {
            var ad = new TypeDiversion(new DiversionDiviner(), old, @new);
            ad.MemberDiversions.ShouldNotBeNull();
        }

        [Theory, AutoMoqData]
        public void AttributeDiversionsShouldBeSetProperly(ITypeInfo old, ITypeInfo @new)
        {
            var ad = new TypeDiversion(new DiversionDiviner(), old, @new);
            ad.AttributeDiversions.ShouldNotBeNull();
        }
    }
}
