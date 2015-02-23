using System;
using System.Collections.Generic;
using System.Reflection;
using Diversion.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

namespace Diversion.Test.Reflection
{
    [TestClass]
    public class NvTypeInfoFacts
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
            _factory.GetInfo(typeof(NvTypeReferenceFacts)).DeclaringType.Should().Equal(null);
        }

        [TestMethod]
        public void DeclaringTypeShouldBeSetIfReferencedTypeIsANestedType()
        {
            _factory.GetInfo(typeof(SampleNestedType)).DeclaringType.Should()
                 .Equal(_factory.GetReference(typeof(NvTypeInfoFacts)));
        }

        [TestMethod]
        public void NameShouldBeCorrectlySet()
        {
            _factory.GetInfo(typeof(NvTypeReferenceFacts))
                .Name.Should().Equal(typeof(NvTypeReferenceFacts).Name);
        }

        [TestMethod]
        public void NamespaceShouldBeCorrectlySet()
        {
            _factory.GetInfo(typeof(NvTypeReferenceFacts))
                .Namespace.Should().Equal(typeof(NvTypeReferenceFacts).Namespace);
        }

        [TestMethod]
        public void IdentityShouldBeCorrectlySet()
        {
            _factory.GetInfo(typeof(NvTypeReferenceFacts))
                .Identity.Should().Equal("Diversion.Test.Reflection.NvTypeReferenceFacts");
        }

        [TestMethod]
        public void IdentityOfNestedTypeShouldBeCorrectlySet()
        {
            _factory.GetInfo(typeof(SampleNestedType))
                .Identity.Should().Equal("Diversion.Test.Reflection.NvTypeInfoFacts+SampleNestedType");
        }

        [TestMethod]
        public void NameOfConstructedGenericTypeShouldBeCorrectlySet()
        {
            _factory.GetInfo(typeof(EventHandler<EventArgs>))
                .Name.Should().Equal("EventHandler<System.EventArgs>");
        }

        [TestMethod]
        public void NameOfGenericTypeDefinitionShouldBeCorrectlySet()
        {
            _factory.GetInfo(typeof(EventHandler<>))
                .Name.Should().Equal("EventHandler<>");
        }

        [TestMethod]
        public void IsInterfaceShouldBeTrueForInterfaces()
        {
            _factory.GetInfo(typeof (IEnumerable<string>)).IsInterface.Should().Be.True();
        }

        [TestMethod]
        public void IsInterfaceShouldBeFalseForClasses()
        {
            _factory.GetInfo(typeof(string)).IsInterface.Should().Be.False();
        }

        [TestMethod]
        public void IsInterfaceShouldBeFalseForStructs()
        {
            _factory.GetInfo(typeof(int)).IsInterface.Should().Be.False();
        }

        [TestMethod]
        public void IsAbstractShouldBeTrueForInterfaces()
        {
            _factory.GetInfo(typeof(IEnumerable<string>)).IsAbstract.Should().Be.True();
        }

        [TestMethod]
        public void IsAbstractShouldBeTrueForAbstractClasses()
        {
            _factory.GetInfo(typeof(PropertyInfo)).IsAbstract.Should().Be.True();
        }

        [TestMethod]
        public void IsAbstractShouldBeFalseForConcreteUnconstructedGenericTypes()
        {
            _factory.GetInfo(typeof(List<>)).IsAbstract.Should().Be.False();
        }
        [TestMethod]
        public void IsAbstractShouldBeFalseForConcreteClasses()
        {
            _factory.GetInfo(typeof(string)).IsAbstract.Should().Be.False();
        }

        class SampleNestedType
        {
        }
    }
}
