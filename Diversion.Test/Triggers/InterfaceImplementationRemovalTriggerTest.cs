using Diversion.Reflection;
using Diversion.Triggers;
using Xunit;
using Moq;
using Shouldly;

namespace Diversion.Test.Triggers
{

    public class InterfaceImplementationRemovalTriggerTest
    {
        [Fact]
        public void ShouldTriggerIfAnInterfaceImplementationIsRemovedFromAType()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(tcs =>
                    tcs.Diverged == new[] {
                        // API Surface Type with removed interface.
                        Mock.Of<ITypeDiversion>(tc =>
                        tc.New == Mock.Of<ITypeInfo>(tcn => tcn.IsOnApiSurface == true) &&
                        tc.InterfaceDiversions == Mock.Of<ICollectionDiversions<ITypeReference>>(ics =>
                            ics.Removed == new []{ Mock.Of<ITypeReference>()})),
                        // Non API Surface Type with removed interface.
                        Mock.Of<ITypeDiversion>(tc =>
                        tc.New == Mock.Of<ITypeInfo>(tcn => tcn.IsOnApiSurface == false) &&
                        tc.InterfaceDiversions == Mock.Of<ICollectionDiversions<ITypeReference>>(ics =>
                            ics.Removed == new []{ Mock.Of<ITypeReference>()}))
                    }));

            new InterfaceImplementationRemovalTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [Fact]
        public void ShouldNotTriggerIfAnInterfaceImplementationIsNotRemovedFromAType()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(tcs =>
                    tcs.Diverged == new[] {
                        //API Surface Type without removed interface.
                        Mock.Of<ITypeDiversion>(tc =>
                            tc.New == Mock.Of<ITypeInfo>(tcn => tcn.IsOnApiSurface == true) &&
                            tc.InterfaceDiversions == Mock.Of<ICollectionDiversions<ITypeReference>>(ics =>
                                ics.Removed == new ITypeReference[0])),
                        // Non API Surface Type with removed interface
                        Mock.Of<ITypeDiversion>(tc =>
                            tc.New == Mock.Of<ITypeInfo>(tcn => tcn.IsOnApiSurface == false) &&
                            tc.InterfaceDiversions == Mock.Of<ICollectionDiversions<ITypeReference>>(ics =>
                                ics.Removed == new []{ Mock.Of<ITypeReference>()}))
                    }));
            new InterfaceImplementationRemovalTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
