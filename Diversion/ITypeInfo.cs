using System;
using System.Collections.Generic;

namespace Diversion
{
    public interface ITypeInfo : IMemberInfo
    {
        bool IsAbstract { get; }
        bool IsInterface { get; }
        string Namespace { get; }
        ITypeInfo Base { get; }
        IEnumerable<ITypeInfo> Interfaces { get; }
        IEnumerable<IMemberInfo> Members { get; }
    }
}