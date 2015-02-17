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
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(tcs =>
                    tcs.Diverged == new[] {
                        Mock.Of<ITypeDiversion>(tc =>
                            tc.InterfaceDiversions == Mock.Of<ICollectionDiversions<ITypeReference>>(ics =>
                                ics.Added == new []{ Mock.Of<ITypeReference>()}))
                    }));
            new NewInterfaceImplementationTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [TestMethod]
        public void ShouldNotTriggerIfAnInterfaceImplementationHasNotBeenAddedToAnyType()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(tcs =>
                    tcs.Diverged == new[] {
                        Mock.Of<ITypeDiversion>(tc =>
                            tc.InterfaceDiversions == Mock.Of<ICollectionDiversions<ITypeReference>>(ics =>
                                ics.Added == new ITypeReference[0]))
                    }));
            new NewInterfaceImplementationTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
