using System;
using Diversion.Reflection;
using Moq;
using Should;
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

        IAssemblyChange ReleaseToBuild { get; set; }
        NextVersion NextVersion { get; set; }

        [Given(@"the newly built assembly is identical to the released assembly")]
        public void GivenTheNewlyBuiltAssemblyIsIdenticalToTheReleasedAssembly()
        {
            ReleaseToBuild = Mock.Of<IAssemblyChange>(ac =>
                ac.New == Mock.Of<IAssemblyInfo>(obj => obj.MD5 == new byte[] { 0 }) &&
                ac.Old == Mock.Of<IAssemblyInfo>(obj => obj.MD5 == new byte[] { 0 }));
        }

        [Given(@"the currently released assembly version number is (.*)\.(.*)\.(.*)")]
        public void GivenTheCurrentlyReleasedAssemblyVersionNumberIs_(int major, int minor, int patch)
        {
            Mock.Get(ReleaseToBuild.Old).SetupGet(obj => obj.Version).Returns(new Version(major, minor, patch));
        }

        [Given(@"the newly built assembly is not identical to the released assembly")]
        public void GivenTheNewlyBuiltAssemblyIsNotIdenticalToTheReleasedAssembly()
        {
            ReleaseToBuild = Mock.Of<IAssemblyChange>(ac =>
                ac.New == Mock.Of<IAssemblyInfo>(obj => obj.MD5 == new byte[] { 0 }) &&
                ac.Old == Mock.Of<IAssemblyInfo>(obj => obj.MD5 == new byte[] { 1 }));
        }

        [Given(@"NextVersion has been initialized with major and minor version triggers that never trigger")]
        public void GivenNextVersionHasBeenInitializedWithMajorAndMinorVersionTriggersThatNeverTrigger()
        {
            NextVersion = new NextVersion()
                .WithMajorTriggers(Mock.Of<IVersionTrigger>(obj => obj.IsTriggered(It.IsAny<IAssemblyChange>()) == false))
                .WithMinorTriggers(Mock.Of<IVersionTrigger>(obj => obj.IsTriggered(It.IsAny<IAssemblyChange>()) == false));
        }

        [Given(@"NextVersion has been initialized with a major version trigger that always triggers and a minor version trigger that never triggers")]
        public void GivenNextVersionHasBeenInitializedWithAMajorVersionTriggerThatAlwaysTriggersAndAMinorVersionTriggerThatNeverTriggers()
        {
            NextVersion = new NextVersion()
                .WithMajorTriggers(Mock.Of<IVersionTrigger>(obj => obj.IsTriggered(It.IsAny<IAssemblyChange>()) == true))
                .WithMinorTriggers(Mock.Of<IVersionTrigger>(obj => obj.IsTriggered(It.IsAny<IAssemblyChange>()) == false));
        }

        [Given(@"NextVersion has been initialized with a major version trigger that never triggers and a minor version trigger that always triggers")]
        public void GivenNextVersionHasBeenInitializedWithAMajorVersionTriggerThatNeverTriggersAndAMinorVersionTriggerThatAlwaysTriggers()
        {
            NextVersion = new NextVersion()
                .WithMajorTriggers(Mock.Of<IVersionTrigger>(obj => obj.IsTriggered(It.IsAny<IAssemblyChange>()) == false))
                .WithMinorTriggers(Mock.Of<IVersionTrigger>(obj => obj.IsTriggered(It.IsAny<IAssemblyChange>()) == true));
        }

        [Given(@"NextVersion has been initialized with a major and minor version triggers that always trigger")]
        public void GivenNextVersionHasBeenInitializedWithAMajorAndMinorVersionTriggersThatAlwaysTrigger()
        {
            NextVersion = new NextVersion()
                .WithMajorTriggers(Mock.Of<IVersionTrigger>(obj => obj.IsTriggered(It.IsAny<IAssemblyChange>()) == true))
                .WithMinorTriggers(Mock.Of<IVersionTrigger>(obj => obj.IsTriggered(It.IsAny<IAssemblyChange>()) == true));
        }

        [Then(@"NextVersion should determine that the next version number should be (.*)\.(.*)\.(.*)")]
        public void ThenNextVersionShouldDetermineThatTheNextVersionNumberShouldBe_(int major, int minor, int patch)
        {
            NextVersion.Determine(ReleaseToBuild).ShouldEqual(new Version(major, minor, patch));
        }
    }
}
