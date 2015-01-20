using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;

namespace Diversion.Test
{
    [TestClass]
    public class NewInterfaceImplementationOnInterfaceTriggerTest
    {
        [TestMethod]
        public void ShouldTriggerIfAnInterfaceImplementationIsAddedToAnInterface()
        {
            var change = Mock.Of<IAssemblyChange>(obj =>
                obj.TypeChanges == Mock.Of<IChanges<ITypeInfo, ITypeChange>>(tcs =>
                    tcs.Changes == new[] {
                        Mock.Of<ITypeChange>(tc =>
                            tc.New == Mock.Of<ITypeInfo>(ti => ti.IsInterface == true) &&
                            tc.Old == Mock.Of<ITypeInfo>(ti => ti.IsInterface == true) &&
                            tc.InterfaceChanges == Mock.Of<ICollectionChanges<ITypeInfo>>(ics =>
                                ics.Added == new []{ Mock.Of<ITypeInfo>()}))
                    }));
            new NewInterfaceImplementationOnInterfaceTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [TestMethod]
        public void ShouldNotTriggerIfAnInterfaceImplementationIsNotAddedToAnInterface()
        {
            var change = Mock.Of<IAssemblyChange>(obj =>
                obj.TypeChanges == Mock.Of<IChanges<ITypeInfo, ITypeChange>>(tcs =>
                    tcs.Changes == new[] {
                        Mock.Of<ITypeChange>(tc =>
                            tc.New == Mock.Of<ITypeInfo>(ti => ti.IsInterface == true) &&
                            tc.Old == Mock.Of<ITypeInfo>(ti => ti.IsInterface == true) &&
                            tc.InterfaceChanges == Mock.Of<ICollectionChanges<ITypeInfo>>(ics =>
                                ics.Added == new ITypeInfo[0])),
                        Mock.Of<ITypeChange>(tc =>
                            tc.New == Mock.Of<ITypeInfo>(ti => ti.IsInterface == false) &&
                            tc.Old == Mock.Of<ITypeInfo>(ti => ti.IsInterface == false) &&
                            tc.InterfaceChanges == Mock.Of<ICollectionChanges<ITypeInfo>>(ics =>
                                ics.Added == new []{ Mock.Of<ITypeInfo>()}))
                    }));
            new NewInterfaceImplementationOnInterfaceTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
