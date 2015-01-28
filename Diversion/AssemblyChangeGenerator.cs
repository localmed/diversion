using System;
using System.Collections.Generic;
using System.Linq;
using Diversion.Reflection;

namespace Diversion
{
    public class AssemblyChangeGenerator
    {
        public IAssemblyChange GetAssemblyChange(IAssemblyInfo old, IAssemblyInfo @new)
        {
            return new AssemblyChange(old, @new);
        }
    }

    internal class AssemblyChange : ChangeImpl<IAssemblyInfo>, IAssemblyChange
    {
        private readonly Lazy<IChanges<ITypeInfo, ITypeChange>> _typeChanges;

        public AssemblyChange(IAssemblyInfo old, IAssemblyInfo @new) : base(old, @new)
        {
            _typeChanges = new Lazy<IChanges<ITypeInfo, ITypeChange>>(() => ChangesGenerator.GenerateChanges(old.Types, @new.Types,
                (o, n) => new TypeChange(o, n)), true);
        }

        public IChanges<ITypeInfo, ITypeChange> TypeChanges
        {
            get { return _typeChanges.Value; }
        }
    }

    internal class TypeChange : ChangeImpl<ITypeInfo>, ITypeChange
    {
        private readonly Lazy<IChanges<IMemberInfo>> _memberChanges;
        private readonly Lazy<ICollectionChanges<ITypeInfo>> _interfaceChanges;

        public TypeChange(ITypeInfo old, ITypeInfo @new)
            : base(old, @new)
        {
            _memberChanges = new Lazy<IChanges<IMemberInfo>>(() => ChangesGenerator.GenerateChanges(old.Members, @new.Members), true);
            _interfaceChanges = new Lazy<ICollectionChanges<ITypeInfo>>(() => ChangesGenerator.GenerateCollectionChanges(old.Interfaces, @new.Interfaces), true);
        }

        public IChanges<IMemberInfo> MemberChanges
        {
            get { return _memberChanges.Value; }
        }

        public ICollectionChanges<ITypeInfo> InterfaceChanges
        {
            get { return _interfaceChanges.Value; }
        }
    }

    internal class ChangeImpl<T> : IChange<T>
    {
        public ChangeImpl(T old, T @new)
        {
            Old = old;
            New = @new;
        }
        public T Old { get; private set; }

        public T New { get; private set; }
    }

    public static class ChangesGenerator
    {
        public static IChanges<T, TC> GenerateChanges<T, TC>(IEnumerable<T> old, IEnumerable<T> @new, Func<T, T, TC> change)
            where TC : IChange<T>
        {
            return new ChangesImpl<T, TC>(old, @new, change);
        }

        public static IChanges<T> GenerateChanges<T>(IEnumerable<T> old, IEnumerable<T> @new)
        {
            return new ChangesImpl<T>(old, @new, (o, n) => new ChangeImpl<T>(o, n));
        }

        public static ICollectionChanges<T> GenerateCollectionChanges<T>(IEnumerable<T> old, IEnumerable<T> @new)
        {
            return new CollectionChangesImpl<T>(old, @new);
        }

        public static IItemChanges<T, TC> GenerateItemChanges<T, TC>(IEnumerable<T> old, IEnumerable<T> @new,
            Func<T, T, TC> change) where TC : IChange<T>
        {
            return new ChangesImpl<T, TC>(old, @new, change);
        }

        private class CollectionChangesImpl<T> : ICollectionChanges<T>
        {
            public CollectionChangesImpl(IEnumerable<T> old, IEnumerable<T> @new)
            {
            }

            public IEnumerable<T> Added { get; private set; }

            public IEnumerable<T> Removed { get; private set; }
        }

        private class ChangesImpl<T, TC> : IChanges<T, TC> where TC : IChange<T>
        {
            public ChangesImpl(IEnumerable<T> old, IEnumerable<T> @new, Func<T, T, TC> change)
            {
                var collectionChanges = GenerateCollectionChanges(old, @new);
                Added = collectionChanges.Added;
                Removed = collectionChanges.Removed;
                Changes = old.Except(Removed).Zip(@new.Except(Added), change).Where(c => c != null).ToArray();
            }

            public IEnumerable<T> Added
            {
                get; private set;
            }

            public IEnumerable<T> Removed
            {
                get;private set;
            }

            public IEnumerable<TC> Changes
            {
                get;private set;
            }
        }

        private class ChangesImpl<T> : ChangesImpl<T, IChange<T>>, IChanges<T>
        {
            public ChangesImpl(IEnumerable<T> old, IEnumerable<T> @new, Func<T, T, IChange<T>> change)
                : base(old, @new, change)
            {
            }
        }
    }

}
