using Diversion.Reflection;
using Diversion.Triggers;
using Xunit;
using Moq;
using Shouldly;

namespace Diversion.Test.Triggers
{

    public class NewInterfaceImplementationTriggerTest
    {
        [Fact]
        public void ShouldTriggerIfAnInterfaceImplementationHasBeenAddedToAnyType()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(tcs =>
                    tcs.Diverged == new[] {
                        Mock.Of<ITypeDiversion>(tc =>
                            tc.New.IsOnApiSurface == true &&
                            tc.InterfaceDiversions == Mock.Of<ICollectionDiversions<ITypeReference>>(ics =>
                                ics.Added == new []{ Mock.Of<ITypeReference>()}))
                    }));
            new NewInterfaceImplementationTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [Fact]
        public void ShouldNotTriggerIfAnInterfaceImplementationHasNotBeenAddedToAnyType()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(tcs =>
                    tcs.Diverged == new[] {
                        Mock.Of<ITypeDiversion>(tc =>
                            tc.New.IsOnApiSurface == true &&
                            tc.InterfaceDiversions == Mock.Of<ICollectionDiversions<ITypeReference>>(ics =>
                                ics.Added == new ITypeReference[0]))
                    }));
            new NewInterfaceImplementationTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
