using System.Collections.Generic;

namespace Diversion.Reflection
{
    public interface IAttributeInfo
    {
        ITypeReference Type { get; }
        IReadOnlyList<IAttributeArgumentInfo> Arguments { get; }
    }
}