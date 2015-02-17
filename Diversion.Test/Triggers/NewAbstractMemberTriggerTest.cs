using Diversion.Reflection;
using Diversion.Triggers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;

namespace Diversion.Test.Triggers
{
    [TestClass]
    public class NewAbstractMemberTriggerTest
    {
        [TestMethod]
        public void ShouldTriggerIfAnyAbstractMembersOfAnyPublicTypesHaveBeenAdded()
        {
            var change = Mock.Of<IAssemblyDiversion>(
                obj => obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(
                    tcs => tcs.Diverged == new [] {Mock.Of<ITypeDiversion>(
                        tc => tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(
                            mc => mc.Added == new[] {Mock.Of<IPropertyInfo>(mi => mi.IsAbstract == true)}))}));
            new NewAbstractMemberTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [TestMethod]
        public void ShouldNotTriggerIfNoAbstractMembersOfAnyTypesHaveBeenAdded()
        {
            var change = Mock.Of<IAssemblyDiversion>(
                obj => obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(
                    tcs => tcs.Diverged == new[] {Mock.Of<ITypeDiversion>(
                        tc => tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(
                            mc => mc.Added == new[] {Mock.Of<IPropertyInfo>()}))}));
            new NewAbstractMemberTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
