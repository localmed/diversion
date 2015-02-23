using System;
using System.Collections.Generic;

namespace Diversion
{
    public interface IDiversionDiviner
    {
        ICollectionDiversions<T> DivineCollectionDiversions<T>(IEnumerable<T> old, IEnumerable<T> @new);
        IDiversions<T> DivineDiversions<T>(IEnumerable<T> old, IEnumerable<T> @new);
        IDiversions<T, TC> DivineDiversions<T, TC>(IEnumerable<T> old, IEnumerable<T> @new, Func<T, T, TC> subdiviner)
            where TC : IDiversion<T>;
        IItemDiversions<T, TC> DivineItemDiversions<T, TC>(IEnumerable<T> old, IEnumerable<T> @new, Func<T, T, TC> subdiviner)
            where TC : IDiversion<T>;
    }
}