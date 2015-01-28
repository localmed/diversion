using System;
using System.Collections.Generic;
using System.Linq;
using Diversion.Reflection;

namespace Diversion
{
    internal class NvAssemblyInfo : IAssemblyInfo
    {
        private readonly Lazy<IEnumerable<ITypeInfo>> _types;

        public NvAssemblyInfo(string name, Version version, Version frameworkVersion, byte[] md5, IEnumerable<ITypeInfo> types)
        {
            Name = name;
            Version = version;
            FrameworkVersion = frameworkVersion;
            MD5 = md5;
            _types = new Lazy<IEnumerable<ITypeInfo>>(types.ToArray);
        }

        public string Name
        {
            get;
            private set;
        }

        public Version Version
        {
            get;
            private set;
        }

        public Version FrameworkVersion
        {
            get;
            private set;
        }

        public byte[] MD5
        {
            get;
            private set;
        }

        public IEnumerable<ITypeInfo> Types
        {
            get { return _types.Value; }
        }

        public IEnumerable<IAttributeInfo> Attributes { get; private set; }
    }

}
