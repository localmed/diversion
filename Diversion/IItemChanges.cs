using System.Collections.Generic;

namespace Diversion
{
    public interface IItemChanges<out T, out TC> where TC : IChange<T>
    {
        IReadOnlyList<TC> Changes { get; }
    }
}