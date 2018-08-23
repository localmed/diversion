using Diversion.Reflection;
using Diversion.Triggers;
using Xunit;
using Moq;
using Shouldly;

namespace Diversion.Test.Triggers
{

    public class NewInterfaceImplementationOnInterfaceTriggerTest
    {
        [Fact]
        public void ShouldTriggerIfAnInterfaceImplementationIsAddedToAnInterface()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(tcs =>
                    tcs.Diverged == new[] {
                        Mock.Of<ITypeDiversion>(tc =>
                            tc.New == Mock.Of<ITypeInfo>(ti => ti.IsInterface && ti.IsOnApiSurface) &&
                            tc.Old == Mock.Of<ITypeInfo>(ti => ti.IsInterface && ti.IsOnApiSurface) &&
                            tc.InterfaceDiversions == Mock.Of<ICollectionDiversions<ITypeReference>>(ics =>
                                ics.Added == new []{ Mock.Of<ITypeReference>()}))
                    }));
            new NewInterfaceImplementationOnInterfaceTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [Fact]
        public void ShouldNotTriggerIfAnInterfaceImplementationIsNotAddedToAnInterface()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(tcs =>
                    tcs.Diverged == new[] {
                        Mock.Of<ITypeDiversion>(tc =>
                            tc.New == Mock.Of<ITypeInfo>(ti => ti.IsInterface && ti.IsOnApiSurface) &&
                            tc.Old == Mock.Of<ITypeInfo>(ti => ti.IsInterface && ti.IsOnApiSurface) &&
                            tc.InterfaceDiversions == Mock.Of<ICollectionDiversions<ITypeReference>>(ics =>
                                ics.Added == new ITypeReference[0])),
                        Mock.Of<ITypeDiversion>(tc =>
                            tc.New == Mock.Of<ITypeInfo>(ti => !ti.IsInterface && ti.IsOnApiSurface) &&
                            tc.Old == Mock.Of<ITypeInfo>(ti => !ti.IsInterface && ti.IsOnApiSurface) &&
                            tc.InterfaceDiversions == Mock.Of<ICollectionDiversions<ITypeReference>>(ics =>
                                ics.Added == new []{ Mock.Of<ITypeReference>()}))
                    }));
            new NewInterfaceImplementationOnInterfaceTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
