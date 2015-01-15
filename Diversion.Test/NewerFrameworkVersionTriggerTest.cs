using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;

namespace Diversion.Test
{
    [TestClass]
    public class NewerFrameworkVersionTriggerTest
    {
        [TestMethod]
        public void IfRecentlyBuiltAssemblyIsBuiltAgainstALaterFrameworkThenTrigger()
        {
            var change = Mock.Of<IAssemblyChange>(obj =>
                obj.New == Mock.Of<IAssemblyInfo>(ai => ai.FrameworkVersion == new Version(4, 5)) &&
                obj.Old == Mock.Of<IAssemblyInfo>(ai => ai.FrameworkVersion == new Version(4, 0)));
            new NewerFrameworkVersionTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [TestMethod]
        public void IfRecentlyBuiltAssemblyIsBuiltAgainstAnEarlierFrameworkThenDoNotTrigger()
        {
            var change = Mock.Of<IAssemblyChange>(obj =>
                obj.New == Mock.Of<IAssemblyInfo>(ai => ai.FrameworkVersion == new Version(4, 0)) &&
                obj.Old == Mock.Of<IAssemblyInfo>(ai => ai.FrameworkVersion == new Version(4, 5)));
            new NewerFrameworkVersionTrigger().IsTriggered(change).ShouldBeFalse();
        }

        [TestMethod]
        public void IfRecentlyBuiltAssemblyIsBuiltAgainstTheSameFrameworkThenDoNotTrigger()
        {
            var change = Mock.Of<IAssemblyChange>(obj =>
                obj.New == Mock.Of<IAssemblyInfo>(ai => ai.FrameworkVersion == new Version(4, 0)) &&
                obj.Old == Mock.Of<IAssemblyInfo>(ai => ai.FrameworkVersion == new Version(4, 0)));
            new NewerFrameworkVersionTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
