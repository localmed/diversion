using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;

namespace Diversion.Test
{
    [TestClass]
    public class NewAbstractMemberTriggerTest
    {
        [TestMethod]
        public void ShouldTriggerIfAnyAbstractMembersOfAnyPublicTypesHaveBeenAdded()
        {
            var change = Mock.Of<IAssemblyChange>(
                obj => obj.TypeChanges == Mock.Of<IChanges<ITypeInfo, ITypeChange>>(
                    tcs => tcs.Changes == new [] {Mock.Of<ITypeChange>(
                        tc => tc.MemberChanges == Mock.Of<IChanges<IMemberInfo, IMemberChange>>(
                            mc => mc.Added == new[] {Mock.Of<IMemberInfo>(mi => mi.IsAbstract == true)}))}));
            new NewAbstractMemberTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [TestMethod]
        public void ShouldNotTriggerIfNoAbstractMembersOfAnyTypesHaveBeenAdded()
        {
            var change = Mock.Of<IAssemblyChange>(
                obj => obj.TypeChanges == Mock.Of<IChanges<ITypeInfo, ITypeChange>>(
                    tcs => tcs.Changes == new[] {Mock.Of<ITypeChange>(
                        tc => tc.MemberChanges == Mock.Of<IChanges<IMemberInfo, IMemberChange>>(
                            mc => mc.Added == new[] {Mock.Of<IMemberInfo>()}))}));
            new NewAbstractMemberTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
