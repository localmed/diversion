using System;
using System.IO;
using System.Linq;
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

        public IAssemblyDiversion Divine(string oldAssemblyLocation, string newAssemblyLocation)
        {
            if(oldAssemblyLocation == null) throw new ArgumentNullException(nameof(oldAssemblyLocation), $"{nameof(oldAssemblyLocation)} must not be null.");
            if(newAssemblyLocation == null) throw new ArgumentNullException(nameof(newAssemblyLocation), $"{nameof(newAssemblyLocation)} must not be null.");

            if (!File.Exists(oldAssemblyLocation)) throw new ArgumentException($"{nameof(oldAssemblyLocation)} must exist.", nameof(oldAssemblyLocation));
            if (!File.Exists(newAssemblyLocation)) throw new ArgumentException($"{nameof(newAssemblyLocation)} must exist.", nameof(newAssemblyLocation));
            var identical = new FileInfo(oldAssemblyLocation).Length == new FileInfo(newAssemblyLocation).Length && File.ReadAllBytes(oldAssemblyLocation).SequenceEqual(File.ReadAllBytes(newAssemblyLocation));
            var released = _assemblyInfoFactory.FromFile(oldAssemblyLocation);
            return new AssemblyDiversion(_diviner, released, identical ? released : _assemblyInfoFactory.FromFile(newAssemblyLocation));
        }
    }
}