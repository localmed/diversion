using System;
using System.Collections.Generic;
using System.Linq;

namespace Diversion
{
    public class DiversionDiviner : IDiversionDiviner
    {
        public IDiversions<T, TC> DivineDiversions<T, TC>(IEnumerable<T> old, IEnumerable<T> @new, Func<T, T, TC> subdiviner)
            where TC : IDiversion<T>
        {
            return new DiversionsImpl<T, TC>(this, old, @new, subdiviner);
        }

        public IDiversions<T> DivineDiversions<T>(IEnumerable<T> old, IEnumerable<T> @new)
        {
            return new DiversionsImpl<T>(this, old, @new, (o, n) => new DiversionBase<T>(o, n));
        }

        public ICollectionDiversions<T> DivineCollectionDiversions<T>(IEnumerable<T> old, IEnumerable<T> @new)
        {
            return new CollectionDiversionsImpl<T>(old, @new);
        }

        public IItemDiversions<T, TC> DivineItemDiversions<T, TC>(IEnumerable<T> old, IEnumerable<T> @new, Func<T, T, TC> subdiviner)
            where TC : IDiversion<T>
        {
            return new DiversionsImpl<T, TC>(this, old, @new, subdiviner);
        }

        private class CollectionDiversionsImpl<T> : ICollectionDiversions<T>
        {
            public CollectionDiversionsImpl(IEnumerable<T> old, IEnumerable<T> @new)
            {
                var n = @new as T[] ?? @new.ToArray();
                var o = old as T[] ?? old.ToArray();
                Removed = o.Except(n).ToArray();
                Added = n.Except(o).ToArray();
            }

            public IReadOnlyList<T> Added { get; private set; }

            public IReadOnlyList<T> Removed { get; private set; }
        }

        private class DiversionsImpl<T, TC> : IDiversions<T, TC>
            where TC : IDiversion<T>
        {
            public DiversionsImpl(IDiversionDiviner diviner, IEnumerable<T> old, IEnumerable<T> @new, Func<T, T, TC> subdiviner)
            {
                var o = old as T[] ?? old.ToArray();
                var n = @new as T[] ?? @new.ToArray();
                var collectionChanges = diviner.DivineCollectionDiversions(o, n);
                Added = collectionChanges.Added;
                Removed = collectionChanges.Removed;
                Diverged = o.Except(Removed).Zip(n.Except(Added), subdiviner).Where(c => c.HasDiverged()).ToArray();
            }

            public IReadOnlyList<T> Added { get; private set; }

            public IReadOnlyList<T> Removed { get; private set; }

            public IReadOnlyList<TC> Diverged { get; private set; }
        }

        private class DiversionsImpl<T> : DiversionsImpl<T, IDiversion<T>>, IDiversions<T>
        {
            public DiversionsImpl(IDiversionDiviner diviner, IEnumerable<T> old, IEnumerable<T> @new, Func<T, T, IDiversion<T>> subdiviner)
                : base(diviner, old, @new, subdiviner)
            {
            }
        }
    }
}