using System;
using System.Collections.Generic;

namespace Diversion
{
    public interface ITypeInfo
    {
        string Name { get; }
        IEnumerable<Type> Interfaces { get; }
        IEnumerable<IMemberInfo> Members { get; }
    }
}