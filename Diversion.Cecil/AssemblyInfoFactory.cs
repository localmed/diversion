using Diversion.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Diversion.Cecil
{
    public class AssemblyInfoFactory : IAssemblyInfoFactory
    {
        public IAssemblyInfo FromFile(string assemblyPath)
        {
            return new AssemblyInfo(assemblyPath);
        }
    }
}
