using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;

namespace Diversion.Test
{
    [TestClass]
    public class NewMemberTriggerTest
    {
        [TestMethod]
        public void ShouldTriggerIfAnyMemberWereAddedToPublicTypes()
        {
            var change = Mock.Of<IAssemblyChange>(obj => 
                obj.TypeChanges == Mock.Of<IChanges<ITypeInfo, ITypeChange>>(tcs => 
                    tcs.Changes == new [] {
                        Mock.Of<ITypeChange>(tc => 
                            tc.MemberChanges == Mock.Of<IChanges<IMemberInfo>>(mc => 
                                mc.Added == new[] {Mock.Of<IMemberInfo>()}))
                    }));
            new NewMemberTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [TestMethod]
        public void ShouldNotTriggerIfNoMembersWereAddedToPublicTypes()
        {
            var change = Mock.Of<IAssemblyChange>(obj =>
                obj.TypeChanges == Mock.Of<IChanges<ITypeInfo, ITypeChange>>(tcs =>
                    tcs.Changes == new[] {
                        Mock.Of<ITypeChange>(tc =>
                            tc.MemberChanges == Mock.Of<IChanges<IMemberInfo>>(mc =>
                                mc.Added == new IMemberInfo[0]))
                    }));
            new NewMemberTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
