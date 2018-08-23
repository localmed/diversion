using Diversion.Reflection;
using Diversion.Triggers;
using Xunit;
using Moq;
using Shouldly;

namespace Diversion.Test.Triggers
{

    public class NewMemberOnInterfaceTriggerTest
    {
        [Fact]
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

        [Fact]
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
