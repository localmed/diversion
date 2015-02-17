using System.Collections.Generic;

namespace Diversion.Reflection
{
    public interface ITypeInfo : IMemberInfo
    {
        bool IsAbstract { get; }
        bool IsInterface { get; }
        string Namespace { get; }
        ITypeReference Base { get; }
        IReadOnlyList<ITypeReference> Interfaces { get; }
        IReadOnlyList<IMemberInfo> Members { get; }
        bool IsGenericType { get; }
        IReadOnlyList<ITypeReference> GenericArguments { get; }
    }
}