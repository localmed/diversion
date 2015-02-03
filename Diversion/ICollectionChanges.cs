using System.Collections.Generic;

namespace Diversion
{
    public interface ICollectionChanges<out T>
    {
        IReadOnlyList<T> Added { get; }
        IReadOnlyList<T> Removed { get; }
    }
}