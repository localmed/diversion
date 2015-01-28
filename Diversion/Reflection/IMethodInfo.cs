using System.Collections.Generic;

namespace Diversion.Reflection
{
    public interface IMethodInfo : IMemberInfo, IVirtualizable
    {
        IEnumerable<IParameterInfo> Parameters { get; }
        IEnumerable<IGenericParameterInfo> GenericParameters { get; }
        IParameterInfo ReturnType { get; }
    }
}