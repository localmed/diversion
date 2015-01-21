using System;
using System.Collections.Generic;
using System.Linq;

namespace Diversion
{
    internal class NvTypeInfo : ITypeInfo
    {
        private readonly Type _type;
        public NvTypeInfo(Type type)
        {
            _type = type;
        }

        public string Name { get; private set; }

        public bool IsPublic { get; private set; }

        public bool IsStatic { get; private set; }

        public bool IsAbstract { get; private set; }

        public bool IsInterface { get; private set; }

        public string Namespace { get; private set; }

        public ITypeInfo Base { get; private set; }

        public IEnumerable<ITypeInfo> Interfaces { get; private set; }

        public IEnumerable<IMemberInfo> Members { get; private set; }

        public IEnumerable<IAttributeInfo> Attributes
        {
            get { return new IAttributeInfo[0];  }
        }
    }
}