using Diversion.Reflection;
using Diversion.Triggers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;

namespace Diversion.Test.Triggers
{
    [TestClass]
    public class InterfaceImplementationRemovalTriggerTest
    {
        [TestMethod]
        public void ShouldTriggerIfAnInterfaceImplementationIsRemovedFromAType()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(tcs =>
                    tcs.Diverged == new[] {Mock.Of<ITypeDiversion>(tc =>
                        tc.InterfaceDiversions == Mock.Of<ICollectionDiversions<ITypeReference>>(ics =>
                            ics.Removed == new []{ Mock.Of<ITypeReference>()}))}));
            new InterfaceImplementationRemovalTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [TestMethod]
        public void ShouldNotTriggerIfAnInterfaceImplementationIsNotRemovedFromAType()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(tcs =>
                    tcs.Diverged == new[] {Mock.Of<ITypeDiversion>(tc =>
                        tc.InterfaceDiversions == Mock.Of<ICollectionDiversions<ITypeReference>>(ics =>
                            ics.Removed == new ITypeReference[0]))}));
            new InterfaceImplementationRemovalTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
