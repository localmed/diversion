using System;
using Diversion.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Diversion.Test
{
    [TestClass]
    public class ReflectionInfoFactoryTest
    {
        [TestMethod]
        public void TestObject()
        {
            var factory = new NvReflectionInfoFactory();
            var type = factory.FromReflection(typeof (object));
            type.IsPublic.ShouldBeTrue();
            type.Name.ShouldEqual("Object");
            type.IsAbstract.ShouldBeFalse();
            type.Interfaces.ShouldBeEmpty();
            type.ToString().ShouldEqual("System.Object");
        }
    }
}
