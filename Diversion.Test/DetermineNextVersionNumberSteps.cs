using System;
using Diversion.Reflection;
using Moq;
using NuGet.Versioning;
using Shouldly;
using TechTalk.SpecFlow;

namespace Diversion.Test
{
    [Binding]
    public class DetermineNextVersionNumberSteps
    {
        public DetermineNextVersionNumberSteps()
        {
            NextVersion = new NextVersion();
        }

        IAssemblyDiversion ReleaseToBuild { get; set; }
        NextVersion NextVersion { get; set; }

        [Given(@"the currently released assembly version number is (.*)\.(.*)\.(.*)")]
        public void GivenTheCurrentlyReleasedAssemblyVersionNumberIs_(int major, int minor, int patch)
        {
            ReleaseToBuild = Mock.Of<IAssemblyDiversion>(ac =>
                ac.New == Mock.Of<IAssemblyInfo>() &&
                ac.Old == Mock.Of<IAssemblyInfo>(obj => obj.Version == new NuGetVersion(major, minor, patch)) &&
                ac.HasDiverged());
        }

        [Given(@"NextVersion has been initialized with major and minor version triggers that never trigger")]
        public void GivenNextVersionHasBeenInitializedWithMajorAndMinorVersionTriggersThatNeverTrigger()
        {
            NextVersion = new NextVersion()
                .WithMajorTriggers(Mock.Of<IVersionTrigger>(obj => obj.IsTriggered(It.IsAny<IAssemblyDiversion>()) == false))
                .WithMinorTriggers(Mock.Of<IVersionTrigger>(obj => obj.IsTriggered(It.IsAny<IAssemblyDiversion>()) == false));
        }

        [Given(@"NextVersion has been initialized with a major version trigger that always triggers and a minor version trigger that never triggers")]
        public void GivenNextVersionHasBeenInitializedWithAMajorVersionTriggerThatAlwaysTriggersAndAMinorVersionTriggerThatNeverTriggers()
        {
            NextVersion = new NextVersion()
                .WithMajorTriggers(Mock.Of<IVersionTrigger>(obj => obj.IsTriggered(It.IsAny<IAssemblyDiversion>()) == true))
                .WithMinorTriggers(Mock.Of<IVersionTrigger>(obj => obj.IsTriggered(It.IsAny<IAssemblyDiversion>()) == false));
        }

        [Given(@"NextVersion has been initialized with a major version trigger that never triggers and a minor version trigger that always triggers")]
        public void GivenNextVersionHasBeenInitializedWithAMajorVersionTriggerThatNeverTriggersAndAMinorVersionTriggerThatAlwaysTriggers()
        {
            NextVersion = new NextVersion()
                .WithMajorTriggers(Mock.Of<IVersionTrigger>(obj => obj.IsTriggered(It.IsAny<IAssemblyDiversion>()) == false))
                .WithMinorTriggers(Mock.Of<IVersionTrigger>(obj => obj.IsTriggered(It.IsAny<IAssemblyDiversion>()) == true));
        }

        [Given(@"NextVersion has been initialized with a major and minor version triggers that always trigger")]
        public void GivenNextVersionHasBeenInitializedWithAMajorAndMinorVersionTriggersThatAlwaysTrigger()
        {
            NextVersion = new NextVersion()
                .WithMajorTriggers(Mock.Of<IVersionTrigger>(obj => obj.IsTriggered(It.IsAny<IAssemblyDiversion>()) == true))
                .WithMinorTriggers(Mock.Of<IVersionTrigger>(obj => obj.IsTriggered(It.IsAny<IAssemblyDiversion>()) == true));
        }

        [Then(@"NextVersion should determine that the next version number should be (.*)\.(.*)\.(.*)")]
        public void ThenNextVersionShouldDetermineThatTheNextVersionNumberShouldBe_(int major, int minor, int patch)
        {
            NextVersion.Determine(ReleaseToBuild).ShouldBe(new NuGetVersion(major, minor, patch));
        }
    }
}
