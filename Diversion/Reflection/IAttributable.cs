using System.Collections.Generic;

namespace Diversion.Reflection
{
    public interface IAttributable
    {
        IReadOnlyList<IAttributeInfo> Attributes { get; }
    }
}