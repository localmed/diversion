using System.Collections.Generic;

namespace Diversion
{
    public interface IPropertyInfo : IMemberInfo, IVirtualizable
    {
        IEnumerable<IParameterInfo> Parameters { get; }
        ITypeInfo Type { get; }
    }
}