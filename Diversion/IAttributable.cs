using System.Collections.Generic;

namespace Diversion
{
    public interface IAttributable
    {
        IEnumerable<IAttributeInfo> Attributes { get; }
    }
}