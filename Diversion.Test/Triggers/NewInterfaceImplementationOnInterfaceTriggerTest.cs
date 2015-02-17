using Diversion.Reflection;
using Diversion.Triggers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;

namespace Diversion.Test.Triggers
{
    [TestClass]
    public class NewInterfaceImplementationOnInterfaceTriggerTest
    {
        [TestMethod]
        public void ShouldTriggerIfAnInterfaceImplementationIsAddedToAnInterface()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(tcs =>
                    tcs.Diverged == new[] {
                        Mock.Of<ITypeDiversion>(tc =>
                            tc.New == Mock.Of<ITypeInfo>(ti => ti.IsInterface == true) &&
                            tc.Old == Mock.Of<ITypeInfo>(ti => ti.IsInterface == true) &&
                            tc.InterfaceDiversions == Mock.Of<ICollectionDiversions<ITypeReference>>(ics =>
                                ics.Added == new []{ Mock.Of<ITypeReference>()}))
                    }));
            new NewInterfaceImplementationOnInterfaceTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [TestMethod]
        public void ShouldNotTriggerIfAnInterfaceImplementationIsNotAddedToAnInterface()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(tcs =>
                    tcs.Diverged == new[] {
                        Mock.Of<ITypeDiversion>(tc =>
                            tc.New == Mock.Of<ITypeInfo>(ti => ti.IsInterface == true) &&
                            tc.Old == Mock.Of<ITypeInfo>(ti => ti.IsInterface == true) &&
                            tc.InterfaceDiversions == Mock.Of<ICollectionDiversions<ITypeReference>>(ics =>
                                ics.Added == new ITypeReference[0])),
                        Mock.Of<ITypeDiversion>(tc =>
                            tc.New == Mock.Of<ITypeInfo>(ti => ti.IsInterface == false) &&
                            tc.Old == Mock.Of<ITypeInfo>(ti => ti.IsInterface == false) &&
                            tc.InterfaceDiversions == Mock.Of<ICollectionDiversions<ITypeReference>>(ics =>
                                ics.Added == new []{ Mock.Of<ITypeReference>()}))
                    }));
            new NewInterfaceImplementationOnInterfaceTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
