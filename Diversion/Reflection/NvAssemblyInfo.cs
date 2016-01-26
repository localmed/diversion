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
    class NvAssemblyInfo : IAssemblyInfo
    {
        public NvAssemblyInfo(string assemblyPath)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);
            var reflectionInfoFactory = new NvReflectionInfoFactory();
            Name = assembly.FullName;
            Attributes = assembly.GetCustomAttributesData().Select(reflectionInfoFactory.GetInfo).ToArray();

            Version = Attributes.Where(attr => Equals(attr.Type, reflectionInfoFactory.GetReference(typeof(AssemblyInformationalVersionAttribute)))).Select(
                attr =>
                {
                    var match = Regex.Match((string) attr.Arguments[0].Value, @"([0-9]+\.[0-9]+\.[0-9]+).?");
                    return match.Success ? new Version(match.Result("$1")) : null;
                }).FirstOrDefault() ?? assembly.GetName().Version;
            FrameworkVersion = Attributes.Where(attr => Equals(attr.Type, reflectionInfoFactory.GetReference(typeof(TargetFrameworkAttribute)))).Select(
                attr =>
                {
                    var match = Regex.Match((string) attr.Arguments[0].Value, @"\.NETFramework,Version=v(.*)");
                    return match.Success ? new Version(match.Result("$1")) : null;
                }).FirstOrDefault() ?? new Version(assembly.ImageRuntimeVersion.Substring(1));
            Types = assembly.GetExportedTypes().AsParallel().Select(reflectionInfoFactory.GetInfo).OrderBy(i => i.Identity).ToArray();
        }

        public string Name { get; private set; }

        public Version Version { get; private set; }

        public Version FrameworkVersion { get; private set; }

        public IReadOnlyList<ITypeInfo> Types { get; private set; }

        public IReadOnlyList<IAttributeInfo> Attributes { get; private set; }
    }
}
