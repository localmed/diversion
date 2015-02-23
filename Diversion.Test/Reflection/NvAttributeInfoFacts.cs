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
                .Type.Should().Equal(_factory.GetReference(typeof (DisplayNameAttribute)));
        }


        [TestMethod]
        public void ArgumentsOfAttributeShouldBeCorrectlySet()
        {
            _factory.GetInfo(typeof (Sample<>).GetCustomAttributesData()[0])
                .Arguments.Should().Contain.One(new NvAttributeArgumentInfo("displayName", "Sample"));
        }

        [DisplayName("Sample")]
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
}
