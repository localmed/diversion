using System;
using System.ComponentModel;
using Diversion.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

namespace Diversion.Test.Reflection
{
    [TestClass]
    public class NvAttributeInfoFacts
    {
        private IReflectionInfoFactory _factory;

        [TestInitialize]
        public void Setup()
        {
            _factory = new NvReflectionInfoFactory();
        }

        [TestMethod]
        public void TypeOfAttributeShouldBeCorrectlySet()
        {
            _factory.GetInfo(typeof (Sample<>).GetCustomAttributesData()[0])
                .Type.Should().Equal(_factory.GetReference(typeof (SampleAttribute)));
        }

        [TestMethod]
        public void ArgumentsOfAttributeShouldBeCorrectlySet()
        {
            var attribute = _factory.GetInfo(typeof(Sample<>).GetCustomAttributesData()[0]);
            attribute.Arguments.Should().Contain.Item(new NvAttributeArgumentInfo("arg1", "hello"));
            attribute.Arguments.Should().Contain.Item(new NvAttributeArgumentInfo("argT", _factory.GetReference(typeof(string))));
            attribute.Arguments.Should().Contain.Item(new NvAttributeArgumentInfo("Names", new[] { "S2", "S3", "S4" }));
        }

        [Sample("hello", 1, 3, typeof(string), new [] { typeof(int), typeof(bool), }, new [] { 1,2,3}, Name = "S1", Names = new[] { "S2", "S3", "S4"})]
        class Sample<T>
        {
            public string field;
            public object Property { get; set; }

            public void Method(string param)
            {
                field = param;
                Property = field;
                if (Changed != null)
                    Changed(this, EventArgs.Empty);
            }

            public event EventHandler Changed;
        }

    }

    public class SampleAttribute : Attribute
    {
        public SampleAttribute(string arg1, int arg2, double arg3, Type argT, Type[] argAT, int[] argAP)
        {

        }

        public string Name { get; set; }

        public string[] Names { get; set; }
    }
}
