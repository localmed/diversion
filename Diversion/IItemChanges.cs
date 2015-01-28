using System.Collections.Generic;

namespace Diversion
{
    public interface IItemChanges<out T, out TC> where TC : IChange<T>
    {
        IEnumerable<TC> Changes { get; }
    }
}