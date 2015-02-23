using System;
using System.Collections.Generic;
using System.Linq;
using Diversion.Reflection;

namespace Diversion
{
    public static class DiversionDiviner
    {
        public static IAssemblyDiversion DivineDiversion(IAssemblyInfo old, IAssemblyInfo @new)
        {
            return new AssemblyDiversion(old, @new);
        }

        public static IDiversions<T, TC> DivineDiversions<T, TC>(IEnumerable<T> old, IEnumerable<T> @new, Func<T, T, TC> change)
            where TC : IDiversion<T>
        {
            return new DiversionsImpl<T, TC>(old, @new, change);
        }

        public static IDiversions<T> DivineDiversions<T>(IEnumerable<T> old, IEnumerable<T> @new)
        {
            return new DiversionsImpl<T>(old, @new, (o, n) => new DiversionImpl<T>(o, n));
        }

        public static ICollectionDiversions<T> DivineCollectionDiversions<T>(IEnumerable<T> old, IEnumerable<T> @new)
        {
            return new CollectionDiversionsImpl<T>(old, @new);
        }

        public static IItemDiversions<T, TC> DivineItemDiversions<T, TC>(IEnumerable<T> old, IEnumerable<T> @new, Func<T, T, TC> change)
            where TC : IDiversion<T>
        {
            return new DiversionsImpl<T, TC>(old, @new, change);
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
            public DiversionsImpl(IEnumerable<T> old, IEnumerable<T> @new, Func<T, T, TC> change)
            {
                var o = old as T[] ?? old.ToArray();
                var n = @new as T[] ?? @new.ToArray();
                var collectionChanges = DivineCollectionDiversions(o, n);
                Added = collectionChanges.Added;
                Removed = collectionChanges.Removed;
                Diverged = o.Except(Removed).Zip(n.Except(Added), change).Where(c => c.HasDiverged()).ToArray();
            }

            public IReadOnlyList<T> Added
            {
                get; private set;
            }

            public IReadOnlyList<T> Removed
            {
                get;private set;
            }

            public IReadOnlyList<TC> Diverged
            {
                get;private set;
            }
        }

        private class DiversionsImpl<T> : DiversionsImpl<T, IDiversion<T>>, IDiversions<T>
        {
            public DiversionsImpl(IEnumerable<T> old, IEnumerable<T> @new, Func<T, T, IDiversion<T>> change)
                : base(old, @new, change)
            {
            }
        }
    }
}