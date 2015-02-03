using System;
using Diversion.Reflection;

namespace Diversion
{
    internal class TypeChange : ChangeImpl<ITypeInfo>, ITypeChange
    {
        private readonly Lazy<IChanges<IMemberInfo>> _memberChanges;
        private readonly Lazy<ICollectionChanges<ITypeInfo>> _interfaceChanges;

        public TypeChange(ITypeInfo old, ITypeInfo @new)
            : base(old, @new)
        {
            _memberChanges = new Lazy<IChanges<IMemberInfo>>(() => ChangeDiviner.DivineChanges(old.Members, @new.Members), true);
            _interfaceChanges = new Lazy<ICollectionChanges<ITypeInfo>>(() => ChangeDiviner.DivineCollectionChanges(old.Interfaces, @new.Interfaces), true);
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
}