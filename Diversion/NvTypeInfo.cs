using System;
using System.Collections.Generic;
using System.Linq;

namespace Diversion
{
    internal class NvTypeInfo : ITypeInfo
    {
        public NvTypeInfo(string name, IEnumerable<Type> interfaces, IEnumerable<IMemberInfo> members)
        {
            Name = name;
            Interfaces = interfaces.ToArray();
            Members = members.ToArray();
        }

        public string Name { get; private set; }

        public IEnumerable<Type> Interfaces { get; private set; }

        public IEnumerable<IMemberInfo> Members { get; private set; }
    }
}