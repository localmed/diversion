using System.Collections.Generic;

namespace Diversion.Reflection
{
    public interface IAttributable
    {
        IEnumerable<IAttributeInfo> Attributes { get; }
    }
}