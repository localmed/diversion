using System.Collections.Generic;

namespace Diversion
{
    public interface IItemDiversions<out T, out TC> where TC : IDiversion<T>
    {
        IReadOnlyList<TC> Diverged { get; }
    }
}