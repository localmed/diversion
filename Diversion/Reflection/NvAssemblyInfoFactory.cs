using System;
using System.IO;
using System.Reflection;

namespace Diversion.Reflection
{
    class NvAssemblyInfoFactory : IAssemblyInfoFactory
    {
        static NvAssemblyInfoFactory()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (s, a) => Assembly.Load(a.Name);
        }

        public IAssemblyInfo FromFile(string assemblyPath)
        {
            using (var context = new AppDomainContext(Path.GetDirectoryName(assemblyPath)))
                return (IAssemblyInfo)context.Domain.CreateInstanceFromAndUnwrap(
                    typeof(NvAssemblyInfo).Assembly.Location,
                    typeof(NvAssemblyInfo).FullName,
                    false, BindingFlags.Default, null,
                    new object[] { assemblyPath }, null, null);
        }
    }
}
