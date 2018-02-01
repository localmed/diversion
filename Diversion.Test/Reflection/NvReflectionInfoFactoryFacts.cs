using System;
using System.ComponentModel;
using Diversion.Reflection;
using Xunit;
using Should.Fluent;

namespace Diversion.Test.Reflection
{

    public class NvReflectionInfoFactoryFacts
    {
        private IReflectionInfoFactory _factory;

        public NvReflectionInfoFactoryFacts()
        {
            _factory = new NvReflectionInfoFactory();
        }

        [Fact]
        public void PassingATypeToGetInfoShouldReturnAnInstanceOfNvTypeInfo()
        {
            _factory.GetInfo(typeof (object)).Should().Be.OfType<NvTypeInfo>();
        }

        [Fact]
        public void PassingAFieldInfoToGetInfoShouldReturnAnInstanceOfNvFieldInfo()
        {
            _factory.GetInfo(typeof (Sample<>).GetField("field")).Should().Be.OfType<NvFieldInfo>();
        }

        [Fact]
        public void PassingAPropertyInfoToGetInfoShouldReturnAnInstanceOfNvPropertyInfo()
        {
            _factory.GetInfo(typeof (Sample<>).GetProperty("Property")).Should().Be.OfType<NvPropertyInfo>();
        }

        [Fact]
        public void PassingAConstructorInfoToGetInfoShouldReturnAnInstanceOfNvConstructorInfo()
        {
            _factory.GetInfo(typeof (Sample<>).GetConstructor(Type.EmptyTypes)).Should().Be.OfType<NvConstructorInfo>();
        }

        [Fact]
        public void PassingAnEventInfoToGetInfoShouldReturnAnInstanceOfNvEventInfo()
        {
            _factory.GetInfo(typeof(Sample<>).GetEvent("Changed")).Should().Be.OfType<NvEventInfo>();
        }

        [Fact]
        public void PassingAMethodInfoToGetInfoShouldReturnAnInstanceOfNvMethodInfo()
        {
            _factory.GetInfo(typeof(Sample<>).GetMethod("Method")).Should().Be.OfType<NvMethodInfo>();
        }

        [Fact]
        public void PassingAParameterInfoToGetInfoShouldReturnAnInstanceOfNvParameterInfo()
        {
            _factory.GetInfo(typeof(Sample<>).GetMethod("Method").GetParameters()[0]).Should().Be.OfType<NvParameterInfo>();
        }

        [Fact]
        public void PassingATypeThatRepresentsAGenericParameterToGetReferenceShouldReturnAnInstanceOfNvParameterInfo()
        {
            _factory.GetReference(typeof(Sample<>).GetGenericArguments()[0]).Should().Be.OfType<NvGenericParameterInfo>();
        }

        [Fact]
        public void PassingATypeToGetReferenceShouldReturnAnInstanceOfNvTypeReference()
        {
            _factory.GetReference(typeof(object)).Should().Be.OfType<NvTypeReference>();
        }

        [Fact]
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
