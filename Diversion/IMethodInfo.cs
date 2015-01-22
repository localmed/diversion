using System.Collections.Generic;

namespace Diversion
{
    public interface IMethodInfo : IMemberInfo, IVirtualizable
    {
        IEnumerable<IParameterInfo> Parameters { get; }
        IEnumerable<IGenericParameter> GenericParameters { get; }
        IParameterInfo ReturnType { get; }
    }
}