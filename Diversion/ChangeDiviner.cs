using System;
using System.Collections.Generic;
using System.Linq;
using Diversion.Reflection;

namespace Diversion
{
    public static class ChangeDiviner
    {
        public static IAssemblyChange DivineChange(IAssemblyInfo old, IAssemblyInfo @new)
        {
            return new AssemblyChange(old, @new);
        }

        public static IChanges<T, TC> DivineChanges<T, TC>(IEnumerable<T> old, IEnumerable<T> @new, Func<T, T, TC> change)
            where T : IIdentifiable
            where TC : IChange<T>
        {
            return new ChangesImpl<T, TC>(old, @new, change);
        }

        public static IChanges<T> DivineChanges<T>(IEnumerable<T> old, IEnumerable<T> @new)
            where T : IIdentifiable
        {
            return new ChangesImpl<T>(old, @new, (o, n) => new ChangeImpl<T>(o, n));
        }

        public static ICollectionChanges<T> DivineCollectionChanges<T>(IEnumerable<T> old, IEnumerable<T> @new)
            where T : IIdentifiable
        {
            return new CollectionChangesImpl<T>(old, @new);
        }

        public static IItemChanges<T, TC> DivineItemChanges<T, TC>(IEnumerable<T> old, IEnumerable<T> @new, Func<T, T, TC> change)
            where T : IIdentifiable
            where TC : IChange<T>
        {
            return new ChangesImpl<T, TC>(old, @new, change);
        }

        private class CollectionChangesImpl<T> : ICollectionChanges<T> 
            where T : IIdentifiable
        {
            public CollectionChangesImpl(IEnumerable<T> old, IEnumerable<T> @new)
            {
                var n = @new as T[] ?? @new.ToArray();
                var o = old as T[] ?? old.ToArray();
                Removed = n.Except(o, new IdentityComparer<T>()).ToArray();
                Added = o.Except(n, new IdentityComparer<T>()).ToArray();
            }

            public IReadOnlyList<T> Added { get; private set; }

            public IReadOnlyList<T> Removed { get; private set; }
        }

        private class ChangesImpl<T, TC> : IChanges<T, TC>
            where T : IIdentifiable
            where TC : IChange<T>
        {
            public ChangesImpl(IEnumerable<T> old, IEnumerable<T> @new, Func<T, T, TC> change)
            {
                var o = old as T[] ?? old.ToArray();
                var n = @new as T[] ?? @new.ToArray();
                var collectionChanges = DivineCollectionChanges(o, n);
                Added = collectionChanges.Added;
                Removed = collectionChanges.Removed;
                Changes = o.Except(Removed).Zip(n.Except(Added), change).Where(c => c != null).ToArray();
            }

            public IReadOnlyList<T> Added
            {
                get; private set;
            }

            public IReadOnlyList<T> Removed
            {
                get;private set;
            }

            public IReadOnlyList<TC> Changes
            {
                get;private set;
            }
        }

        private class ChangesImpl<T> : ChangesImpl<T, IChange<T>>, IChanges<T>
            where T : IIdentifiable
        {
            public ChangesImpl(IEnumerable<T> old, IEnumerable<T> @new, Func<T, T, IChange<T>> change)
                : base(old, @new, change)
            {
            }
        }
    }
}