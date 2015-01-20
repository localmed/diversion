using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;

namespace Diversion.Test
{
    [TestClass]
    public class NewTypeTriggerTest
    {
        [TestMethod]
        public void ShouldTriggerIfAnyPublicTypesHaveBeenAdded()
        {
            var change = Mock.Of<IAssemblyChange>(obj =>
                obj.TypeChanges == Mock.Of<IChanges<ITypeInfo, ITypeChange>>(tc =>
                    tc.Added == new[] {Mock.Of<ITypeInfo>()}));
            new NewTypeTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [TestMethod]
        public void ShouldNotTriggerIfNoPublicTypesHaveBeenAdded()
        {
            var change = Mock.Of<IAssemblyChange>(obj =>
                obj.TypeChanges == Mock.Of<IChanges<ITypeInfo, ITypeChange>>(tc =>
                    tc.Added == new ITypeInfo[0]));
            new NewTypeTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
