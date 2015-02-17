using Diversion.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

namespace Diversion.Test
{
    [TestClass]
    public class ReflectionInfoFactoryTest
    {
        [TestMethod]
        public void TestObject()
        {
            var factory = new NvReflectionInfoFactory();
            var type = factory.GetInfo(typeof (object));
            type.IsPublic.Should().Be.True();
            type.Name.Should().Equal("Object");
            type.IsAbstract.Should().Be.False();
            type.Interfaces.Should().Be.Empty();
            type.ToString().Should().Equal("System.Object");
            DiversionDiviner.DivineDiversions(new[] { factory.GetInfo(typeof(NextVersion)) }, new[] {
                    factory.GetInfo(typeof (NextVersion))}).HasDiverged().Should().Be.False();
            factory.GetInfo(typeof (NextVersion)).IsInterface.Should().Be.False();
        }

        //[TestMethod]
        //public void IntegrationTest()
        //{
        //    var factory = new NvReflectionInfoFactory();
        //    var assemblyChange = DiversionDiviner.DivineDiversion(
        //        factory.FromFile(@"C:\Users\Mike\Projects\diversion\Diversion\bin\Release\Diversion.dll"),
        //        factory.FromFile(@"C:\Users\Mike\Projects\diversion\Diversion\bin\Debug\Diversion.dll"));
        //    new NextVersion().Determine(assemblyChange).Should().Equal(assemblyChange.Old.Version);
        //}
    }
}
