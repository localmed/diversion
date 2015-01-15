﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;

namespace Diversion.Test
{
    [TestClass]
    public class TypeRemovalTriggerTest
    {
        [TestMethod]
        public void ShouldTriggerIfAnyPublicTypesHaveBeenRemoved()
        {
            var change = Mock.Of<IAssemblyChange>(
                obj => obj.TypeChanges == Mock.Of<IChanges<ITypeInfo, ITypeChange>>(
                    tc => tc.Removed == new[] {Mock.Of<ITypeInfo>()}));
            new TypeRemovalTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [TestMethod]
        public void ShouldNotTriggerIfNoPublicTypesHaveBeenRemoved()
        {
            var change = Mock.Of<IAssemblyChange>(
                obj => obj.TypeChanges == Mock.Of<IChanges<ITypeInfo, ITypeChange>>(
                    tc => tc.Removed == new ITypeInfo[0]));
            new TypeRemovalTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
