using System.Collections.Generic;

namespace Diversion
{
    public interface IChanges<out T> : IChanges<T, IChange<T>> { }

    public interface IChanges<out T, out TC> where TC : IChange<T>
    {
        IEnumerable<T> Added { get; }
        IEnumerable<T> Removed { get; }
        IEnumerable<TC> Changes { get; }
    }
}