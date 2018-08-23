using Diversion.Reflection;
using Diversion.Triggers;
using Xunit;
using Moq;
using Shouldly;

namespace Diversion.Test.Triggers
{

    public class MemberRemovalTriggerTest
    {
        [Fact]
        public void ShouldTriggerIfAnyPublicMembersOfAnyPublicTypesHaveBeenRemoved()
        {
            var change = Mock.Of<IAssemblyDiversion>(
                obj => obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(
                    tcs => tcs.Diverged == new [] {
                        // API Surface type with removed API Surface member.
                        Mock.Of<ITypeDiversion>(tc =>
                            tc.New == Mock.Of<ITypeInfo>(t => t.IsOnApiSurface == true) &&
                            tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(
                                mc =>
                                mc.Diverged == new IDiversion<IMemberInfo>[0] &&
                                mc.Removed == new[] {Mock.Of<IMemberInfo>( m => m.IsOnApiSurface == true)}))}));
            new MemberRemovalTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [Fact]
        public void ShouldNotTriggerIfNoPublicMembersOfAnyTypesHaveBeenRemoved()
        {
            var change = Mock.Of<IAssemblyDiversion>(
                obj => obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(
                    tcs => tcs.Diverged == new[] {
                        // API Surface type without removed API Surface member.
                        Mock.Of<ITypeDiversion>(tc =>
                            tc.New == Mock.Of<ITypeInfo>(t => t.IsOnApiSurface == true) &&
                            tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(
                                mc =>
                                mc.Diverged == new IDiversion<IMemberInfo>[0] &&
                                mc.Removed == new IMemberInfo[0]))}));
            new MemberRemovalTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
