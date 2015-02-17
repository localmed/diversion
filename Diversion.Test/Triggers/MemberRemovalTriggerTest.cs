using Diversion.Reflection;
using Diversion.Triggers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;

namespace Diversion.Test.Triggers
{
    [TestClass]
    public class MemberRemovalTriggerTest
    {
        [TestMethod]
        public void ShouldTriggerIfAnyPublicMembersOfAnyPublicTypesHaveBeenRemoved()
        {
            var change = Mock.Of<IAssemblyDiversion>(
                obj => obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(
                    tcs => tcs.Diverged == new [] {Mock.Of<ITypeDiversion>(
                        tc => tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(
                            mc => mc.Removed == new[] {Mock.Of<IMemberInfo>()}))}));
            new MemberRemovalTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [TestMethod]
        public void ShouldNotTriggerIfNoPublicMembersOfAnyTypesHaveBeenRemoved()
        {
            var change = Mock.Of<IAssemblyDiversion>(
                obj => obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(
                    tcs => tcs.Diverged == new[] {Mock.Of<ITypeDiversion>(
                        tc => tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(
                            mc => mc.Removed == new IMemberInfo[0]))}));
            new MemberRemovalTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
