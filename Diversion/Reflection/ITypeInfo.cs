using System.Collections.Generic;

namespace Diversion.Reflection
{
    public interface ITypeInfo : IMemberInfo
    {
        bool IsAbstract { get; }
        bool IsInterface { get; }
        string Namespace { get; }
        ITypeInfo Base { get; }
        IReadOnlyList<ITypeInfo> Interfaces { get; }
        IReadOnlyList<IMemberInfo> Members { get; }
        bool IsGenericType { get; }
        IReadOnlyList<ITypeInfo> GenericArguments { get; }
    }
}