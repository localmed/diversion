using Diversion.Reflection;
using Diversion.Triggers;
using Xunit;
using Moq;
using Should;

namespace Diversion.Test.Triggers
{
    
    public class NewTypeTriggerTest
    {
        [Fact]
        public void ShouldTriggerIfAnyPublicTypesHaveBeenAdded()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(tc =>
                    tc.Added == new[] {Mock.Of<ITypeInfo>(t => t.IsOnApiSurface)}));
            new NewTypeTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [Fact]
        public void ShouldNotTriggerIfNoPublicTypesHaveBeenAdded()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(tc =>
                    tc.Diverged == new ITypeDiversion[0] &&
                    tc.Added == new ITypeInfo[0]));
            new NewTypeTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
