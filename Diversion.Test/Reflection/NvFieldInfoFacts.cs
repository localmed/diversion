using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diversion.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Diversion.Test.Reflection
{
    [TestClass]
    public class NvFieldInfoFacts
    {
        private IReflectionInfoFactory _factory;

        [TestInitialize]
        public void Setup()
        {
            _factory = new NvReflectionInfoFactory();
        }
    }
}
