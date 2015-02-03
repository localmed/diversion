using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

namespace Diversion.Reflection
{
    class NvAssemblyInfo : IAssemblyInfo
    {
        private readonly Assembly _assembly;
        private readonly Lazy<Version> _version;
        private readonly Lazy<Version> _frameworkVersion;
        private readonly Lazy<IReadOnlyList<ITypeInfo>> _types;
        private readonly Lazy<IReadOnlyList<IAttributeInfo>> _attributes;

        public NvAssemblyInfo(IReflectionInfoFactory reflectionInfoFactory, Assembly assembly, byte[] md5)
        {
            _assembly = assembly;
            _attributes = new Lazy<IReadOnlyList<IAttributeInfo>>(_assembly.GetCustomAttributesData().Select(reflectionInfoFactory.FromReflection).ToArray, true);
            _version = new Lazy<Version>(() => Attributes.Where(attr => attr.Type.Member == typeof(AssemblyInformationalVersionAttribute)).Select(attr => new Version((string)attr.Arguments[0].Value)).FirstOrDefault() ?? _assembly.GetName().Version, true);
            _frameworkVersion = new Lazy<Version>(() => Attributes.Where(attr => attr.Type.Member == typeof(TargetFrameworkAttribute))
                .Select(attr => new Version(Regex.Match((string)attr.Arguments[0].Value, @"\.NETFramework,Version=v(.*)").Result("$1"))).FirstOrDefault() ?? new Version(assembly.ImageRuntimeVersion), true);
            _types = new Lazy<IReadOnlyList<ITypeInfo>>(_assembly.GetExportedTypes().AsParallel().Select(reflectionInfoFactory.FromReflection).ToArray, true);
            MD5 = md5;
        }

        public string Name
        {
            get { return _assembly.FullName; }
        }

        public Version Version
        {
            get { return _version.Value; }
        }

        public Version FrameworkVersion
        {
            get { return _frameworkVersion.Value; }
        }

        public byte[] MD5
        {
            get;
            private set;
        }

        public IReadOnlyList<ITypeInfo> Types
        {
            get { return _types.Value; }
        }

        public IReadOnlyList<IAttributeInfo> Attributes
        {
            get { return _attributes.Value; }
        }
    }
}
