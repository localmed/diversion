using System;
using System.Diagnostics.Contracts;
using System.IO;
using Diversion.Reflection;

namespace Diversion
{
    public class AssemblyDiversionDiviner
    {
        private readonly IReflectionInfoFactory _reflectionInfoFactory;
        private readonly IDiversionDiviner _diviner;

        public AssemblyDiversionDiviner(IReflectionInfoFactory reflectionInfoFactory, IDiversionDiviner diviner)
        {
            _reflectionInfoFactory = reflectionInfoFactory;
            _diviner = diviner;
        }

        public AssemblyDiversionDiviner() : this(new NvReflectionInfoFactory(), new DiversionDiviner())
        {
        }

        public IAssemblyDiversion Divine(string oldAssemblyLocation, string newAssemblyLocation)
        {
            //Contract.Requires<ArgumentNullException>(oldAssemblyLocation != null, "oldAssemblyPath must not be null.");
            //Contract.Requires<ArgumentNullException>(newAssemblyLocation != null, "newAssemblyPath must not be null.");
            //Contract.Requires<ArgumentException>(File.Exists(oldAssemblyLocation), "oldAssemblyPath must exist.");
            //Contract.Requires<ArgumentException>(File.Exists(newAssemblyLocation), "newAssemblyPath must exist.");
            return new AssemblyDiversion(_diviner, _reflectionInfoFactory.FromFile(oldAssemblyLocation), _reflectionInfoFactory.FromFile(newAssemblyLocation));
        }
    }
}