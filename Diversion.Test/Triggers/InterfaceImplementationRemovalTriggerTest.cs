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
            var change = Mock.Of<IAssemblyChange>(obj =>
                obj.TypeChanges == Mock.Of<IChanges<ITypeInfo, ITypeChange>>(tcs =>
                    tcs.Changes == new[] {Mock.Of<ITypeChange>(tc =>
                        tc.InterfaceChanges == Mock.Of<ICollectionChanges<ITypeInfo>>(ics =>
                            ics.Removed == new []{ Mock.Of<ITypeInfo>()}))}));
            new InterfaceImplementationRemovalTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [TestMethod]
        public void ShouldNotTriggerIfAnInterfaceImplementationIsNotRemovedFromAType()
        {
            var change = Mock.Of<IAssemblyChange>(obj =>
                obj.TypeChanges == Mock.Of<IChanges<ITypeInfo, ITypeChange>>(tcs =>
                    tcs.Changes == new[] {Mock.Of<ITypeChange>(tc =>
                        tc.InterfaceChanges == Mock.Of<ICollectionChanges<ITypeInfo>>(ics =>
                            ics.Removed == new ITypeInfo[0]))}));
            new InterfaceImplementationRemovalTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
