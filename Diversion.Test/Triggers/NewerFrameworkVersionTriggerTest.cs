using System;
using Diversion.Reflection;
using Diversion.Triggers;
using Xunit;
using Moq;
using Should;

namespace Diversion.Test.Triggers
{
    
    public class NewerFrameworkVersionTriggerTest
    {
        [Fact]
        public void ShouldTriggerIfRecentlyBuiltAssemblyIsBuiltAgainstALaterFramework()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.New == Mock.Of<IAssemblyInfo>(ai => ai.FrameworkVersion == new Version(4, 5)) &&
                obj.Old == Mock.Of<IAssemblyInfo>(ai => ai.FrameworkVersion == new Version(4, 0)));
            new NewerFrameworkVersionTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [Fact]
        public void ShouldNotTriggerIfRecentlyBuiltAssemblyIsBuiltAgainstAnEarlierFramework()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.New == Mock.Of<IAssemblyInfo>(ai => ai.FrameworkVersion == new Version(4, 0)) &&
                obj.Old == Mock.Of<IAssemblyInfo>(ai => ai.FrameworkVersion == new Version(4, 5)));
            new NewerFrameworkVersionTrigger().IsTriggered(change).ShouldBeFalse();
        }

        [Fact]
        public void ShouldNotTriggerIfRecentlyBuiltAssemblyIsBuiltAgainstTheSameFramework()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.New == Mock.Of<IAssemblyInfo>(ai => ai.FrameworkVersion == new Version(4, 0)) &&
                obj.Old == Mock.Of<IAssemblyInfo>(ai => ai.FrameworkVersion == new Version(4, 0)));
            new NewerFrameworkVersionTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
