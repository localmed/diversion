using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Should.Fluent;

namespace Diversion.Reflection.Test
{

    public class NvTypeInfoFacts
    {
        private IReflectionInfoFactory _factory;

        public NvTypeInfoFacts()
        {
            _factory = new NvReflectionInfoFactory();
        }

        [Fact]
        public void DeclaringTypeShouldBeNullIfReferencedTypeIsNotANestedType()
        {
            _factory.GetInfo(typeof(NvTypeReferenceFacts)).DeclaringType.Should().Equal(null);
        }

        [Fact]
        public void DeclaringTypeShouldBeSetIfReferencedTypeIsANestedType()
        {
            _factory.GetInfo(typeof(SampleNestedType)).DeclaringType.Should()
                 .Equal(_factory.GetReference(typeof(NvTypeInfoFacts)));
        }

        [Fact]
        public void NameShouldBeCorrectlySet()
        {
            _factory.GetInfo(typeof(NvTypeReferenceFacts))
                .Name.Should().Equal(typeof(NvTypeReferenceFacts).Name);
        }

        [Fact]
        public void NamespaceShouldBeCorrectlySet()
        {
            _factory.GetInfo(typeof(NvTypeReferenceFacts))
                .Namespace.Should().Equal(typeof(NvTypeReferenceFacts).Namespace);
        }

        [Fact]
        public void IdentityShouldBeCorrectlySet()
        {
            _factory.GetInfo(typeof(NvTypeReferenceFacts))
                .Identity.Should().Equal("Diversion.Reflection.Test.NvTypeReferenceFacts");
        }

        [Fact]
        public void IdentityOfNestedTypeShouldBeCorrectlySet()
        {
            _factory.GetInfo(typeof(SampleNestedType))
                .Identity.Should().Equal("Diversion.Reflection.Test.NvTypeInfoFacts+SampleNestedType");
        }

        [Fact]
        public void NameOfConstructedGenericTypeShouldBeCorrectlySet()
        {
            _factory.GetInfo(typeof(EventHandler<EventArgs>))
                .Name.Should().Equal("EventHandler<System.EventArgs>");
        }

        [Fact]
        public void NameOfGenericTypeDefinitionShouldBeCorrectlySet()
        {
            _factory.GetInfo(typeof(EventHandler<>))
                .Name.Should().Equal("EventHandler<>");
        }

        [Fact]
        public void IsInterfaceShouldBeTrueForInterfaces()
        {
            _factory.GetInfo(typeof (IEnumerable<string>)).IsInterface.Should().Be.True();
        }

        [Fact]
        public void IsInterfaceShouldBeFalseForClasses()
        {
            _factory.GetInfo(typeof(string)).IsInterface.Should().Be.False();
        }

        [Fact]
        public void IsInterfaceShouldBeFalseForStructs()
        {
            _factory.GetInfo(typeof(int)).IsInterface.Should().Be.False();
        }

        [Fact]
        public void IsAbstractShouldBeTrueForInterfaces()
        {
            _factory.GetInfo(typeof(IEnumerable<string>)).IsAbstract.Should().Be.True();
        }

        [Fact]
        public void IsAbstractShouldBeTrueForAbstractClasses()
        {
            _factory.GetInfo(typeof(PropertyInfo)).IsAbstract.Should().Be.True();
        }

        [Fact]
        public void IsAbstractShouldBeFalseForConcreteUnconstructedGenericTypes()
        {
            _factory.GetInfo(typeof(List<>)).IsAbstract.Should().Be.False();
        }
        [Fact]
        public void IsAbstractShouldBeFalseForConcreteClasses()
        {
            _factory.GetInfo(typeof(string)).IsAbstract.Should().Be.False();
        }

        class SampleNestedType
        {
        }
    }
}
