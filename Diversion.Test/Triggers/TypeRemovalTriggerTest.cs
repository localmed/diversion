using Diversion.Reflection;
using Diversion.Triggers;
using Xunit;
using Moq;
using Shouldly;

namespace Diversion.Test.Triggers
{

    public class TypeRemovalTriggerTest
    {
        [Fact]
        public void ShouldTriggerIfAnyPublicTypesHaveBeenRemoved()
        {
            var change = Mock.Of<IAssemblyDiversion>(
                obj => obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(
                    tc => tc.Removed == new[] {Mock.Of<ITypeInfo>(t => t.IsOnApiSurface)}));
            new TypeRemovalTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [Fact]
        public void ShouldNotTriggerIfNoPublicTypesHaveBeenRemoved()
        {
            var change = Mock.Of<IAssemblyDiversion>(
                obj => obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(
                    tc =>
                    tc.Diverged == new ITypeDiversion[0] &&
                    tc.Removed == new ITypeInfo[0]));
            new TypeRemovalTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
