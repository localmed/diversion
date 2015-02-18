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
        [TestMethod]
        public void IsInterfaceShouldBeTrueForInterfaces()
        {
            new NvTypeInfo(new NvReflectionInfoFactory(), typeof (IEnumerable<string>)).IsInterface.Should().Be.True();
        }

        [TestMethod]
        public void IsInterfaceShouldBeFalseForClasses()
        {
            new NvTypeInfo(new NvReflectionInfoFactory(), typeof(string)).IsInterface.Should().Be.False();
        }

        [TestMethod]
        public void IsInterfaceShouldBeFalseForStructs()
        {
            new NvTypeInfo(new NvReflectionInfoFactory(), typeof(int)).IsInterface.Should().Be.False();
        }

        [TestMethod]
        public void IsAbstractShouldBeTrueForInterfaces()
        {
            new NvTypeInfo(new NvReflectionInfoFactory(), typeof (IEnumerable<string>)).IsAbstract.Should().Be.True();
        }

        [TestMethod]
        public void IsAbstractShouldBeTrueForAbstractClasses()
        {
            new NvTypeInfo(new NvReflectionInfoFactory(), typeof (PropertyInfo)).IsAbstract.Should().Be.True();
        }

        [TestMethod]
        public void IsAbstractShouldBeFalseForConcreteUnconstructedGenericTypes()
        {
            new NvTypeInfo(new NvReflectionInfoFactory(), typeof(List<>)).IsAbstract.Should().Be.False();
        }
        [TestMethod]
        public void IsAbstractShouldBeFalseForConcreteClasses()
        {
            new NvTypeInfo(new NvReflectionInfoFactory(), typeof (string)).IsAbstract.Should().Be.False();
        }
    }
}
