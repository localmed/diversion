using NuGet.Frameworks;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;

namespace Diversion.Reflection
{
    [Serializable]
    public class NvAssemblyInfo : IAssemblyInfo
    {
        private Lazy<IReadOnlyList<ITypeInfo>> _types;
        private Lazy<IReadOnlyList<IAttributeInfo>> _attributes;
        private readonly string _version;
        private readonly FrameworkName _targetFramework;

        public NvAssemblyInfo(string assemblyPath)
        {

            var assembly = Assembly.ReflectionOnlyLoadFrom(assemblyPath);
            var reflectionInfoFactory = new NvReflectionInfoFactory();

            Identity = assembly.GetName().Name;
            Name = assembly.FullName;
            Mvid = assembly.ManifestModule.ModuleVersionId;
            _version = assembly.GetNuGetVersion().ToFullString();
            _targetFramework = new FrameworkName(assembly.GetNuGetFramework().DotNetFrameworkName);
            _attributes = new Lazy<IReadOnlyList<IAttributeInfo>>(() => assembly.GetCustomAttributesData().Select(reflectionInfoFactory.GetInfo).ToArray());
            _types = new Lazy<IReadOnlyList<ITypeInfo>>(() => assembly.GetLoadableTypes().Where(t => t.IsPublicOrProtected()).Select(reflectionInfoFactory.GetInfo).OrderBy(i => i.Identity).ToArray());
        }

        public string Identity { get;  }

        public string Name { get; }

        public Guid Mvid { get; }

        public NuGetVersion Version => new NuGetVersion(_version);

        public NuGetFramework TargetFramework => new NuGetFramework(_targetFramework.Identifier, _targetFramework.Version, _targetFramework.Profile);

        public IReadOnlyList<ITypeInfo> Types => _types.Value;

        public IReadOnlyList<IAttributeInfo> Attributes => _attributes.Value;

        public override string ToString() => Identity;
    }

    static class AssemblyExtensions
    {
        public static NuGetFramework GetNuGetFramework(this Assembly it)
        {
            var frameworkNameText = it.GetCustomAttributesData().Where(attr => attr.AttributeType.Name == nameof(TargetFrameworkAttribute))
                .Select(attr => (string)attr.ConstructorArguments[0].Value).FirstOrDefault();
            var frameworkName = new FrameworkName(frameworkNameText);
            return new NuGetFramework(frameworkName.Identifier, frameworkName.Version, frameworkName.Profile);
        }

        public static NuGetVersion GetNuGetVersion(this Assembly it)
        {
            var informationalVersion = it.GetCustomAttributesData().Where(attr => attr.AttributeType.Name == nameof(AssemblyInformationalVersionAttribute))
                .Select(attr => (string)attr.ConstructorArguments[0].Value).FirstOrDefault();
            return informationalVersion != null
                && NuGetVersion.TryParse(informationalVersion, out NuGetVersion version)
                ? version : it.GetName().Version.ToSemanticVersion();
        }
    }
}
