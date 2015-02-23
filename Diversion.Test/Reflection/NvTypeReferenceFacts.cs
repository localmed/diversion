using System;
using Diversion.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

namespace Diversion.Test.Reflection
{
    [TestClass]
    public class NvTypeReferenceFacts
    {
        private IReflectionInfoFactory _factory;

        [TestInitialize]
        public void Setup()
        {
            _factory = new NvReflectionInfoFactory();
        }

        [TestMethod]
        public void DeclaringTypeShouldBeNullIfReferencedTypeIsNotANestedType()
        {
            _factory.GetReference(typeof(NvTypeReferenceFacts)).DeclaringType.Should().Equal(null);
        }

        [TestMethod]
        public void DeclaringTypeShouldBeSetIfReferencedTypeIsANestedType()
        {
            _factory.GetReference(typeof(SampleNestedType)).DeclaringType.Should()
                 .Equal(_factory.GetReference(typeof (NvTypeReferenceFacts)));
        }

        [TestMethod]
        public void NameShouldBeCorrectlySet()
        {
            _factory.GetReference(typeof(NvTypeReferenceFacts))
                .Name.Should().Equal(typeof (NvTypeReferenceFacts).Name);
        }

        [TestMethod]
        public void NameOfConstructedGenericTypeShouldBeCorrectlySet()
        {
            _factory.GetReference(typeof(EventHandler<EventArgs>))
                .Name.Should().Equal("EventHandler<System.EventArgs>");
        }

        [TestMethod]
        public void NameOfGenericTypeDefinitionShouldBeCorrectlySet()
        {
            _factory.GetReference(typeof(EventHandler<>))
                .Name.Should().Equal("EventHandler<>");
        }

        [TestMethod]
        public void NamespaceShouldBeCorrectlySet()
        {
            _factory.GetReference(typeof(NvTypeReferenceFacts))
                .Namespace.Should().Equal(typeof (NvTypeReferenceFacts).Namespace);
        }

        [TestMethod]
        public void IdentityShouldBeCorrectlySet()
        {
            _factory.GetReference(typeof(NvTypeReferenceFacts))
                .Identity.Should().Equal("Diversion.Test.Reflection.NvTypeReferenceFacts");
        }

        [TestMethod]
        public void IdentityOfNestedTypeShouldBeCorrectlySet()
        {
            _factory.GetReference(typeof(SampleNestedType))
                .Identity.Should().Equal("Diversion.Test.Reflection.NvTypeReferenceFacts+SampleNestedType");
        }

        class SampleNestedType
        {
        }
    }
}
