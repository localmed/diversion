using System.Collections.Generic;

namespace Diversion.Reflection
{
    public interface IMethodInfo : IMemberInfo, IVirtualizable
    {
        IReadOnlyList<IParameterInfo> Parameters { get; }
        ITypeReference ReturnType { get; }
        bool IsGenericMethod { get; }
        IReadOnlyList<ITypeReference> GenericArguments { get; }
    }
}