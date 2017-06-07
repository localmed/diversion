using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Diversion.Reflection
{
    class NvAssemblyInfoFactory : IAssemblyInfoFactory
    {
        static NvAssemblyInfoFactory()
        {
            var assemblies = new HashSet<string>();
            AppDomain.CurrentDomain.AssemblyResolve += (s, a) =>
            {
                try
                {
                    return assemblies.Add(a.Name) ? Assembly.Load(a.Name) : null;
                }
                catch { return null; }
            };
        }

        public IAssemblyInfo FromFile(string assemblyPath)
        {
            using (var context = new AppDomainContext(Path.GetDirectoryName(assemblyPath)))
            {
                return (IAssemblyInfo)context.Domain.CreateInstanceFromAndUnwrap(
                    typeof(NvAssemblyInfo).Assembly.Location,
                    typeof(NvAssemblyInfo).FullName,
                    false, BindingFlags.Default, null,
                    new object[] { assemblyPath }, null, null);
            }
        }
    }
}
