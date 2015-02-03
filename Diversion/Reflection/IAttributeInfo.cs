using System.Collections.Generic;

namespace Diversion.Reflection
{
    public interface IAttributeInfo
    {
        ITypeInfo Type { get; }
        IReadOnlyList<IAttributeArgumentInfo> Arguments { get; } 
    }
}