using System.Collections.Generic;

namespace Diversion.Reflection
{
    public interface IMethodInfo : IMemberInfo, IVirtualizable
    {
        IReadOnlyList<IParameterInfo> Parameters { get; }
        IParameterInfo ReturnType { get; }
        bool IsGenericMethod { get; }
        IReadOnlyList<ITypeReference> GenericArguments { get; }
    }
}