using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Diversion.Reflection
{
    public class NvAssemblyInfoFactory : IAssemblyInfoFactory
    {
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
