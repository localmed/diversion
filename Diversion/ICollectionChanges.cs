using System.Collections.Generic;

namespace Diversion
{
    public interface ICollectionChanges<out T>
    {
        IEnumerable<T> Added { get; }
        IEnumerable<T> Removed { get; }
    }
}