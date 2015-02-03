using Diversion.Reflection;
using Diversion.Triggers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;

namespace Diversion.Test.Triggers
{
    [TestClass]
    public class NewInterfaceImplementationTriggerTest
    {
        [TestMethod]
        public void ShouldTriggerIfAnInterfaceImplementationHasBeenAddedToAnyType()
        {
            var change = Mock.Of<IAssemblyChange>(obj =>
                obj.TypeChanges == Mock.Of<IChanges<ITypeInfo, ITypeChange>>(tcs =>
                    tcs.Changes == new[] {
                        Mock.Of<ITypeChange>(tc =>
                            tc.InterfaceChanges == Mock.Of<ICollectionChanges<ITypeInfo>>(ics =>
                                ics.Added == new []{ Mock.Of<ITypeInfo>()}))
                    }));
            new NewInterfaceImplementationTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [TestMethod]
        public void ShouldNotTriggerIfAnInterfaceImplementationHasNotBeenAddedToAnyType()
        {
            var change = Mock.Of<IAssemblyChange>(obj =>
                obj.TypeChanges == Mock.Of<IChanges<ITypeInfo, ITypeChange>>(tcs =>
                    tcs.Changes == new[] {
                        Mock.Of<ITypeChange>(tc =>
                            tc.InterfaceChanges == Mock.Of<ICollectionChanges<ITypeInfo>>(ics =>
                                ics.Added == new ITypeInfo[0]))
                    }));
            new NewInterfaceImplementationTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
