using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

namespace Diversion.Reflection
{
    [Serializable]
    public class NvAssemblyInfo : IAssemblyInfo
    {
        public NvAssemblyInfo(string assemblyPath)
        {
            MD5 = System.Security.Cryptography.MD5.Create().ComputeHash(File.ReadAllBytes(assemblyPath));
            var assembly = Assembly.LoadFrom(assemblyPath);
            var reflectionInfoFactory = new NvReflectionInfoFactory();
            Name = assembly.FullName;
            Attributes = assembly.GetCustomAttributesData().Select(reflectionInfoFactory.GetInfo).ToArray();
            Version = Attributes.Where(attr => Equals(attr.Type, reflectionInfoFactory.GetReference(typeof(AssemblyInformationalVersionAttribute)))).Select(attr => new Version((string)attr.Arguments[0].Value)).FirstOrDefault() ?? assembly.GetName().Version;
            FrameworkVersion = Attributes.Where(attr => Equals(attr.Type, reflectionInfoFactory.GetReference(typeof(TargetFrameworkAttribute)))).Select(
                        attr => new Version(Regex.Match((string)attr.Arguments[0].Value, @"\.NETFramework,Version=v(.*)").Result("$1"))).FirstOrDefault() ?? new Version(assembly.ImageRuntimeVersion);
            Types = assembly.GetExportedTypes().AsParallel().Select(reflectionInfoFactory.GetInfo).ToArray();
        }

        public string Name { get; private set; }

        public Version Version { get; private set; }

        public Version FrameworkVersion { get; private set; }

        public byte[] MD5 { get; private set; }

        public IReadOnlyList<ITypeInfo> Types { get; private set; }

        public IReadOnlyList<IAttributeInfo> Attributes { get; private set; }
    }
}
