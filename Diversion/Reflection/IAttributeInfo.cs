using System.Collections.Generic;

namespace Diversion.Reflection
{
    public interface IAttributeInfo : IIdentifiable
    {
        ITypeReference Type { get; }
        IReadOnlyList<IAttributeArgumentInfo> Arguments { get; }
    }
}