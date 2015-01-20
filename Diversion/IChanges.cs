using System.Collections.Generic;

namespace Diversion
{
    public interface IChanges<out T> : IChanges<T, IChange<T>> { }

    public interface IChanges<out T, out TC> : ICollectionChanges<T> where TC : IChange<T>
    {
        IEnumerable<TC> Changes { get; }
    }

    public interface ICollectionChanges<out T>
    {
        IEnumerable<T> Added { get; }
        IEnumerable<T> Removed { get; }
    }
}