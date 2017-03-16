using Diversion.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

namespace Diversion.Test.Reflection
{
    [TestClass]
    public class NvAttributeArgumentInfoFacts
    {

        [TestMethod]
        public void NameShouldBeSetCorrectly()
        {
            new NvAttributeArgumentInfo("Argument1", null).Name.Should().Equal("Argument1");
        }

        [TestMethod]
        public void ValueShouldBeSetCorrectlyAndAllowedToBeNull()
        {
            new NvAttributeArgumentInfo("Argument1", null).Value.Should().Be.Null();
        }

        [TestMethod]
        public void ValueShouldBeSetCorrectly()
        {
            var value = new object();
            new NvAttributeArgumentInfo("Argument1",  value).Value.Should().Equal(value);
        }
    }
}
