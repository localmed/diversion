using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;

namespace Diversion.Test
{
    [TestClass]
    public class MemberRemovalTriggerTest
    {
        [TestMethod]
        public void ShouldTriggerIfAnyPublicMembersOfAnyPublicTypesHaveBeenRemoved()
        {
            var change = Mock.Of<IAssemblyChange>(
                obj => obj.TypeChanges == Mock.Of<IChanges<ITypeInfo, ITypeChange>>(
                    tcs => tcs.Changes == new [] {Mock.Of<ITypeChange>(
                        tc => tc.MemberChanges == Mock.Of<IChanges<IMemberInfo, IMemberChange>>(
                            mc => mc.Removed == new[] {Mock.Of<IMemberInfo>()}))}));
            new MemberRemovalTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [TestMethod]
        public void ShouldNotTriggerIfNoPublicMembersOfAnyTypesHaveBeenRemoved()
        {
            var change = Mock.Of<IAssemblyChange>(
                obj => obj.TypeChanges == Mock.Of<IChanges<ITypeInfo, ITypeChange>>(
                    tcs => tcs.Changes == new[] {Mock.Of<ITypeChange>(
                        tc => tc.MemberChanges == Mock.Of<IChanges<IMemberInfo, IMemberChange>>(
                            mc => mc.Removed == new IMemberInfo[0]))}));
            new MemberRemovalTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
