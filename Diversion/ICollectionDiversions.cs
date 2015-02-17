using System.Collections.Generic;

namespace Diversion
{
    public interface ICollectionDiversions<out T>
    {
        IReadOnlyList<T> Added { get; }
        IReadOnlyList<T> Removed { get; }
    }
}