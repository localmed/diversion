using System;
using Diversion.Reflection;

namespace Diversion
{
    class TypeDiversion : DiversionBase<ITypeInfo>, ITypeDiversion
    {
        private readonly Lazy<IDiversions<IMemberInfo>> _memberChanges;
        private readonly Lazy<ICollectionDiversions<ITypeReference>> _interfaceChanges;
        private readonly Lazy<ICollectionDiversions<IAttributeInfo>> _attributeDiversions;
        private readonly Lazy<bool> _hasDiverged;

        public TypeDiversion(IDiversionDiviner diviner, ITypeInfo old, ITypeInfo @new)
            : base(old, @new)
        {
            _memberChanges = new Lazy<IDiversions<IMemberInfo>>(() => diviner.DivineDiversions(old.Members, @new.Members), true);
            _interfaceChanges = new Lazy<ICollectionDiversions<ITypeReference>>(() => diviner.DivineCollectionDiversions(old.Interfaces, @new.Interfaces), true);
            _attributeDiversions = new Lazy<ICollectionDiversions<IAttributeInfo>>(() => diviner.DivineCollectionDiversions(old.Attributes, @new.Attributes), true);
            _hasDiverged = new Lazy<bool>(() => base.HasDiverged() || MemberDiversions.HasDiverged() || InterfaceDiversions.HasDiverged() || AttributeDiversions.HasDiverged(), true);
        }

        public IDiversions<IMemberInfo> MemberDiversions
        {
            get { return _memberChanges.Value; }
        }

        public ICollectionDiversions<ITypeReference> InterfaceDiversions
        {
            get { return _interfaceChanges.Value; }
        }

        public ICollectionDiversions<IAttributeInfo> AttributeDiversions
        {
            get { return _attributeDiversions.Value; }
        }

        public override bool HasDiverged()
        {
            return _hasDiverged.Value;
        }

    }
}