using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using Diversion.Reflection;

namespace Diversion.Cecil
{
    [Serializable]
    public class AssemblyInfo : IAssemblyInfo
    {
        public AssemblyInfo(string assemblyPath)
        {
            var assembly = Mono.Cecil.AssemblyDefinition.ReadAssembly(assemblyPath);
            var reflectionInfoFactory = new ReflectionInfoFactory();

            Identity = assembly.Name.Name;
            Name = assembly.FullName;
            Attributes = assembly.CustomAttributes.Select(reflectionInfoFactory.GetInfo).ToArray();

            Version = Attributes.Where(attr => attr.Type.Name == nameof(AssemblyInformationalVersionAttribute)).Select(
                attr =>
                {
                    var match = Regex.Match((string) attr.Arguments[0].Value, @"([0-9]+\.[0-9]+\.[0-9]+).?");
                    return match.Success ? new Version(match.Result("$1")) : null;
                }).FirstOrDefault() ?? assembly.Name.Version;
            FrameworkVersion = Attributes.Where(attr => Equals(attr.Type.Name, nameof(TargetFrameworkAttribute))).Select(
                attr =>
                {
                    var match = Regex.Match((string) attr.Arguments[0].Value, @"\.NETFramework,Version=v(.*)");
                    return match.Success ? new Version(match.Result("$1")) : null;
                }).FirstOrDefault() ?? new Version(assembly.MainModule.RuntimeVersion.Substring(1));
            Types = assembly.MainModule.Types.AsParallel().Select(reflectionInfoFactory.GetInfo).OrderBy(i => i.Identity).ToArray();
        }

        public string Identity { get; private set; }

        public string Name { get; private set; }

        public Version Version { get; private set; }

        public Version FrameworkVersion { get; private set; }

        public IReadOnlyList<ITypeInfo> Types { get; private set; }

        public IReadOnlyList<IAttributeInfo> Attributes { get; private set; }

        public override string ToString() => Identity;
    }
}
