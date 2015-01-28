using System.Collections.Generic;

namespace Diversion.Reflection
{
    public interface IPropertyInfo : IMemberInfo, IVirtualizable
    {
        IEnumerable<IParameterInfo> IndexerParameters { get; }
        ITypeInfo Type { get; }
    }
}