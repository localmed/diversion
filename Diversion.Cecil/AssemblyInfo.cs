using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using Diversion.Reflection;
using Mono.Cecil;
using NuGet.Frameworks;
using NuGet.Versioning;

namespace Diversion.Cecil
{
    public class AssemblyInfo : IAssemblyInfo
    {
        public AssemblyInfo(string assemblyPath)
        {
            var resolver = new DefaultAssemblyResolver();
            var assembly = Mono.Cecil.AssemblyDefinition.ReadAssembly(assemblyPath, new ReaderParameters { AssemblyResolver = resolver } );
            var reflectionInfoFactory = new ReflectionInfoFactory();

            resolver.AddSearchDirectory(Path.GetDirectoryName(assemblyPath));

            Identity = assembly.Name.Name;
            Name = assembly.FullName;
            Mvid = assembly.MainModule.Mvid;
            Attributes = assembly.CustomAttributes.Select(reflectionInfoFactory.GetInfo).ToArray();

            Version = Attributes.Where(attr => attr.Type.Name == nameof(AssemblyInformationalVersionAttribute)).Select(
                attr => NuGetVersion.TryParse((string) attr.Arguments[0].Value, out NuGetVersion version) ? version : null)
                .FirstOrDefault() ?? assembly.Name.Version.ToSemanticVersion();
            TargetFramework = Attributes.Where(attr => Equals(attr.Type.Name, nameof(TargetFrameworkAttribute))).Select(
                attr =>
                {
                    var name = new FrameworkName((string) attr.Arguments[0].Value);
                    return new NuGetFramework(name.Identifier, name.Version, name.Profile);
                }).FirstOrDefault() ?? NuGetFramework.UnsupportedFramework;
            Types = assembly.MainModule.Types.AsParallel().Select(reflectionInfoFactory.GetInfo).Where(t => t.IsOnApiSurface).AsSequential().OrderBy(i => i.Identity).ToArray();
        }

        public string Identity { get; }

        public string Name { get;  }

        public Guid Mvid { get; }

        public NuGetVersion Version { get;  }

        public NuGetFramework TargetFramework { get;  }

        public IReadOnlyList<ITypeInfo> Types { get;  }

        public IReadOnlyList<IAttributeInfo> Attributes { get; }

        public override string ToString() => Identity;
    }
}
