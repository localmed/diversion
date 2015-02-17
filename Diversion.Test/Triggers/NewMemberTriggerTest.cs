using Diversion.Reflection;
using Diversion.Triggers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;

namespace Diversion.Test.Triggers
{
    [TestClass]
    public class NewMemberTriggerTest
    {
        [TestMethod]
        public void ShouldTriggerIfAnyMemberWereAddedToPublicTypes()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj => 
                obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(tcs => 
                    tcs.Diverged == new [] {
                        Mock.Of<ITypeDiversion>(tc => 
                            tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(mc => 
                                mc.Added == new[] {Mock.Of<IMemberInfo>()}))
                    }));
            new NewMemberTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [TestMethod]
        public void ShouldNotTriggerIfNoMembersWereAddedToPublicTypes()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(tcs =>
                    tcs.Diverged == new[] {
                        Mock.Of<ITypeDiversion>(tc =>
                            tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(mc =>
                                mc.Added == new IMemberInfo[0]))
                    }));
            new NewMemberTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
