using Diversion.Reflection;
using Diversion.Triggers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;

namespace Diversion.Test.Triggers
{
    [TestClass]
    public class NewMemberOnInterfaceTriggerTest
    {
        [TestMethod]
        public void ShouldTriggerIfAnyMembersHaveBeenAddedToAnyInterface()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(tcs =>
                    tcs.Diverged == new [] {
                        Mock.Of<ITypeDiversion>(tc =>
                            tc.New.IsInterface &&
                            tc.New.IsOnApiSurface &&
                            tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(mc =>
                                mc.Added == new[] {Mock.Of<IMemberInfo>()}))
                    }));
            new NewMemberOnInterfaceTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [TestMethod]
        public void ShouldTriggerIfNoMembersHaveBeenAddedToAnyInterface()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(tcs =>
                    tcs.Diverged == new[] {
                        Mock.Of<ITypeDiversion>(tc =>
                            tc.New.IsInterface &&
                            tc.New.IsOnApiSurface &&
                            tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(mc =>
                                mc.Added == new IMemberInfo[0])),
                        Mock.Of<ITypeDiversion>(tc =>
                            !tc.New.IsInterface &&
                            tc.New.IsOnApiSurface &&
                            tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(mc =>
                                mc.Added == new[] {Mock.Of<IMemberInfo>()}))
                    }));
            new NewMemberOnInterfaceTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
