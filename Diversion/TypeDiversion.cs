using System;
using Diversion.Reflection;

namespace Diversion
{
    class TypeDiversion : DiversionImpl<ITypeInfo>, ITypeDiversion
    {
        private readonly Lazy<IDiversions<IMemberInfo>> _memberChanges;
        private readonly Lazy<ICollectionDiversions<ITypeReference>> _interfaceChanges;
        private readonly Lazy<bool> _hasDiverged;

        public TypeDiversion(ITypeInfo old, ITypeInfo @new)
            : base(old, @new)
        {
            _memberChanges = new Lazy<IDiversions<IMemberInfo>>(() => DiversionDiviner.DivineDiversions(old.Members, @new.Members), true);
            _interfaceChanges = new Lazy<ICollectionDiversions<ITypeReference>>(() => DiversionDiviner.DivineCollectionDiversions(old.Interfaces, @new.Interfaces), true);
            _hasDiverged = new Lazy<bool>(() => base.HasDiverged() || MemberDiversions.HasDiverged() || InterfaceDiversions.HasDiverged(), true);
        }

        public IDiversions<IMemberInfo> MemberDiversions
        {
            get { return _memberChanges.Value; }
        }

        public ICollectionDiversions<ITypeReference> InterfaceDiversions
        {
            get { return _interfaceChanges.Value; }
        }

        public override bool HasDiverged()
        {
            return _hasDiverged.Value;
        }
    }
}