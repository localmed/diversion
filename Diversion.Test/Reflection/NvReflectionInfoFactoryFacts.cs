using System;
using System.ComponentModel;
using System.Reflection;
using Diversion.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

namespace Diversion.Test.Reflection
{
    [TestClass]
    public class NvReflectionInfoFactoryFacts
    {
        private IReflectionInfoFactory _factory;

        [TestInitialize]
        public void Setup()
        {
            _factory = new NvReflectionInfoFactory();
        }

        [TestMethod]
        public void PassingATypeToGetInfoShouldReturnAnInstanceOfNvTypeInfo()
        {
            _factory.GetInfo(typeof (object)).Should().Be.OfType<NvTypeInfo>();
        }

        [TestMethod]
        public void PassingAFieldInfoToGetInfoShouldReturnAnInstanceOfNvFieldInfo()
        {
            _factory.GetInfo(typeof (Sample<>).GetField("field")).Should().Be.OfType<NvFieldInfo>();
        }

        [TestMethod]
        public void PassingAPropertyInfoToGetInfoShouldReturnAnInstanceOfNvPropertyInfo()
        {
            _factory.GetInfo(typeof (Sample<>).GetProperty("Property")).Should().Be.OfType<NvPropertyInfo>();
        }

        [TestMethod]
        public void PassingAConstructorInfoToGetInfoShouldReturnAnInstanceOfNvConstructorInfo()
        {
            _factory.GetInfo(typeof (Sample<>).GetConstructor(Type.EmptyTypes)).Should().Be.OfType<NvConstructorInfo>();
        }

        [TestMethod]
        public void PassingAnEventInfoToGetInfoShouldReturnAnInstanceOfNvEventInfo()
        {
            _factory.GetInfo(typeof(Sample<>).GetEvent("Changed")).Should().Be.OfType<NvEventInfo>();
        }

        [TestMethod]
        public void PassingAMethodInfoToGetInfoShouldReturnAnInstanceOfNvMethodInfo()
        {
            _factory.GetInfo(typeof(Sample<>).GetMethod("Method")).Should().Be.OfType<NvMethodInfo>();
        }

        [TestMethod]
        public void PassingAParameterInfoToGetInfoShouldReturnAnInstanceOfNvParameterInfo()
        {
            _factory.GetInfo(typeof(Sample<>).GetMethod("Method").GetParameters()[0]).Should().Be.OfType<NvParameterInfo>();
        }

        [TestMethod]
        public void PassingATypeThatRepresentsAGenericParameterToGetReferenceShouldReturnAnInstanceOfNvParameterInfo()
        {
            _factory.GetReference(typeof(Sample<>).GetGenericArguments()[0]).Should().Be.OfType<NvGenericParameterInfo>();
        }

        [TestMethod]
        public void PassingATypeToGetReferenceShouldReturnAnInstanceOfNvTypeReference()
        {
            _factory.GetReference(typeof(object)).Should().Be.OfType<NvTypeReference>();
        }

        [TestMethod]
        public void PassingACustomAttributeDataToGetInfoShouldReturnAnInstanceOfNvAttributeInfo()
        {
            _factory.GetInfo(typeof (Sample<>).GetCustomAttributesData()[0]).Should().Be.OfType<NvAttributeInfo>();
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
