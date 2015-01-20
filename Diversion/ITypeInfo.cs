using System;
using System.Collections.Generic;

namespace Diversion
{
    public interface ITypeInfo
    {
        bool IsInterface { get; }
        string Name { get; }
        IEnumerable<Type> Interfaces { get; }
        IEnumerable<IMemberInfo> Members { get; }
    }
}