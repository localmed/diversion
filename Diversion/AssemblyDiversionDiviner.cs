//using System;
//using System.Diagnostics.Contracts;
//using System.IO;
using Diversion.Reflection;

namespace Diversion
{
    public class AssemblyDiversionDiviner
    {
        private readonly IAssemblyInfoFactory _assemblyInfoFactory;
        private readonly IDiversionDiviner _diviner;

        public AssemblyDiversionDiviner(IAssemblyInfoFactory assemblyInfoFactory, IDiversionDiviner diviner)
        {
            _assemblyInfoFactory = assemblyInfoFactory;
            _diviner = diviner;
        }

        public AssemblyDiversionDiviner() : this(new NvAssemblyInfoFactory(), new DiversionDiviner())
        {
        }

        public IAssemblyDiversion Divine(string oldAssemblyLocation, string newAssemblyLocation)
        {
            //Contract.Requires<ArgumentNullException>(oldAssemblyLocation != null, "oldAssemblyPath must not be null.");
            //Contract.Requires<ArgumentNullException>(newAssemblyLocation != null, "newAssemblyPath must not be null.");
            //Contract.Requires<ArgumentException>(File.Exists(oldAssemblyLocation), "oldAssemblyPath must exist.");
            //Contract.Requires<ArgumentException>(File.Exists(newAssemblyLocation), "newAssemblyPath must exist.");
            return new AssemblyDiversion(_diviner, _assemblyInfoFactory.FromFile(oldAssemblyLocation), _assemblyInfoFactory.FromFile(newAssemblyLocation));
        }
    }
}