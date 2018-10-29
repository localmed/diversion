using Should.Fluent;
using Xunit;

namespace Diversion.Reflection.Test
{

    public class NvAttributeArgumentInfoFacts
    {

        [Fact]
        public void NameShouldBeSetCorrectly()
        {
            new NvAttributeArgumentInfo("Argument1", null).Name.Should().Equal("Argument1");
        }

        [Fact]
        public void ValueShouldBeSetCorrectlyAndAllowedToBeNull()
        {
            new NvAttributeArgumentInfo("Argument1", null).Value.Should().Be.Null();
        }

        [Fact]
        public void ValueShouldBeSetCorrectly()
        {
            var value = new object();
            new NvAttributeArgumentInfo("Argument1",  value).Value.Should().Equal(value);
        }
    }
}
