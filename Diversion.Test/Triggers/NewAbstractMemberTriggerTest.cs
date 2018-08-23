using Diversion.Reflection;
using Diversion.Triggers;
using Xunit;
using Moq;
using Shouldly;

namespace Diversion.Test.Triggers
{

    public class NewAbstractMemberTriggerTest
    {
        [Fact]
        public void ShouldTriggerIfAnyAbstractMembersOfAnyPublicTypesHaveBeenAdded()
        {
            var change = Mock.Of<IAssemblyDiversion>(
                obj => obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(
                    tcs => tcs.Diverged == new [] {Mock.Of<ITypeDiversion>(
                        tc => tc.New.IsOnApiSurface &&
                            tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(
                            mc => mc.Added == new[] {Mock.Of<IPropertyInfo>(mi => mi.IsAbstract)}))}));
            new NewAbstractMemberTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [Fact]
        public void ShouldNotTriggerIfNoAbstractMembersOfAnyTypesHaveBeenAdded()
        {
            var change = Mock.Of<IAssemblyDiversion>(
                obj => obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(
                    tcs => tcs.Diverged == new[] {Mock.Of<ITypeDiversion>(
                        tc => tc.New.IsOnApiSurface &&
                            tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(
                            mc => mc.Added == new[] {Mock.Of<IPropertyInfo>()}))}));
            new NewAbstractMemberTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
