using Diversion.Reflection;
using Diversion.Triggers;
using Xunit;
using Moq;
using Shouldly;

namespace Diversion.Test.Triggers
{

    public class NewMemberTriggerTest
    {
        [Fact]
        public void ShouldTriggerIfAnyMemberWereAddedToPublicTypes()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(tcs =>
                    tcs.Diverged == new [] {
                        Mock.Of<ITypeDiversion>(tc =>
                            tc.New.IsOnApiSurface &&
                            tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(mc =>
                                mc.Added == new[] {Mock.Of<IMemberInfo>(m => m.IsOnApiSurface)}))
                    }));
            new NewMemberTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [Fact]
        public void ShouldNotTriggerIfNoMembersWereAddedToPublicTypes()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(tcs =>
                    tcs.Diverged == new[] {
                        Mock.Of<ITypeDiversion>(tc =>
                            tc.New.IsOnApiSurface &&
                            tc.Old.IsOnApiSurface &&
                            tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(mc =>
                                mc.Diverged == new IDiversion<IMemberInfo>[0] &&
                                mc.Added == new IMemberInfo[0]))
                    }));
            new NewMemberTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
