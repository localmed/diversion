using Diversion.Reflection;
using Diversion.Triggers;
using Xunit;
using Moq;
using Shouldly;
using NuGet.Frameworks;

namespace Diversion.Test.Triggers
{

    public class NewerFrameworkVersionTriggerTest
    {
        [Fact]
        public void ShouldTriggerIfRecentlyBuiltAssemblyIsBuiltAgainstALaterFramework()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.New == Mock.Of<IAssemblyInfo>(ai => ai.TargetFramework == NuGetFramework.Parse("net45")) &&
                obj.Old == Mock.Of<IAssemblyInfo>(ai => ai.TargetFramework == NuGetFramework.Parse("net40")));
            new NewerFrameworkVersionTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [Fact]
        public void ShouldNotTriggerIfRecentlyBuiltAssemblyIsBuiltAgainstAnEarlierFramework()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.New == Mock.Of<IAssemblyInfo>(ai => ai.TargetFramework == NuGetFramework.Parse("net40")) &&
                obj.Old == Mock.Of<IAssemblyInfo>(ai => ai.TargetFramework == NuGetFramework.Parse("net45")));
            new NewerFrameworkVersionTrigger().IsTriggered(change).ShouldBeFalse();
        }

        [Fact]
        public void ShouldNotTriggerIfRecentlyBuiltAssemblyIsBuiltAgainstTheSameFramework()
        {
            var change = Mock.Of<IAssemblyDiversion>(obj =>
                obj.New == Mock.Of<IAssemblyInfo>(ai => ai.TargetFramework == NuGetFramework.Parse("net40")) &&
                obj.Old == Mock.Of<IAssemblyInfo>(ai => ai.TargetFramework == NuGetFramework.Parse("net40")));
            new NewerFrameworkVersionTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
