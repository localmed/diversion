using System;
using Xunit;
using Should.Fluent;

namespace Diversion.Reflection.Test
{

    public class NvTypeReferenceFacts
    {
        private IReflectionInfoFactory _factory;

        public NvTypeReferenceFacts()
        {
            _factory = new NvReflectionInfoFactory();
        }

        [Fact]
        public void DeclaringTypeShouldBeNullIfReferencedTypeIsNotANestedType()
        {
            _factory.GetReference(typeof(NvTypeReferenceFacts)).DeclaringType.Should().Equal(null);
        }

        [Fact]
        public void DeclaringTypeShouldBeSetIfReferencedTypeIsANestedType()
        {
            _factory.GetReference(typeof(SampleNestedType)).DeclaringType.Should()
                 .Equal(_factory.GetReference(typeof (NvTypeReferenceFacts)));
        }

        [Fact]
        public void NameShouldBeCorrectlySet()
        {
            _factory.GetReference(typeof(NvTypeReferenceFacts))
                .Name.Should().Equal(typeof (NvTypeReferenceFacts).Name);
        }

        [Fact]
        public void NameOfConstructedGenericTypeShouldBeCorrectlySet()
        {
            _factory.GetReference(typeof(EventHandler<EventArgs>))
                .Name.Should().Equal("EventHandler<System.EventArgs>");
        }

        [Fact]
        public void NameOfGenericTypeDefinitionShouldBeCorrectlySet()
        {
            _factory.GetReference(typeof(EventHandler<>))
                .Name.Should().Equal("EventHandler<>");
        }

        [Fact]
        public void NamespaceShouldBeCorrectlySet()
        {
            _factory.GetReference(typeof(NvTypeReferenceFacts))
                .Namespace.Should().Equal(typeof (NvTypeReferenceFacts).Namespace);
        }

        [Fact]
        public void IdentityShouldBeCorrectlySet()
        {
            _factory.GetReference(typeof(NvTypeReferenceFacts))
                .Identity.Should().Equal("Diversion.Reflection.Test.NvTypeReferenceFacts");
        }

        [Fact]
        public void IdentityOfNestedTypeShouldBeCorrectlySet()
        {
            _factory.GetReference(typeof(SampleNestedType))
                .Identity.Should().Equal("Diversion.Reflection.Test.NvTypeReferenceFacts+SampleNestedType");
        }

        class SampleNestedType
        {
        }
    }
}
