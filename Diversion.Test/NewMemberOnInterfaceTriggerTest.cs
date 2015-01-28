using Diversion.Reflection;
using Diversion.Triggers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;

namespace Diversion.Test
{
    [TestClass]
    public class NewMemberOnInterfaceTriggerTest
    {
        [TestMethod]
        public void ShouldTriggerIfAnyMembersHaveBeenAddedToAnyInterface()
        {
            var change = Mock.Of<IAssemblyChange>(obj =>
                obj.TypeChanges == Mock.Of<IChanges<ITypeInfo, ITypeChange>>(tcs =>
                    tcs.Changes == new [] {
                        Mock.Of<ITypeChange>(tc =>
                            tc.New.IsInterface == true &&
                            tc.MemberChanges == Mock.Of<IChanges<IMemberInfo>>(mc =>
                                mc.Added == new[] {Mock.Of<IMemberInfo>()}))
                    }));
            new NewMemberOnInterfaceTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [TestMethod]
        public void ShouldTriggerIfNoMembersHaveBeenAddedToAnyInterface()
        {
            var change = Mock.Of<IAssemblyChange>(obj =>
                obj.TypeChanges == Mock.Of<IChanges<ITypeInfo, ITypeChange>>(tcs =>
                    tcs.Changes == new[] {
                        Mock.Of<ITypeChange>(tc =>
                            tc.New.IsInterface == true &&
                            tc.MemberChanges == Mock.Of<IChanges<IMemberInfo>>(mc =>
                                mc.Added == new IMemberInfo[0])),
                        Mock.Of<ITypeChange>(tc =>
                            tc.New.IsInterface == false &&
                            tc.MemberChanges == Mock.Of<IChanges<IMemberInfo>>(mc =>
                                mc.Added == new[] {Mock.Of<IMemberInfo>()}))
                    }));
            new NewMemberOnInterfaceTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
