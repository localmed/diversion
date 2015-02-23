using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
