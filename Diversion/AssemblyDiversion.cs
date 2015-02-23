using System;
using Diversion.Reflection;

namespace Diversion
{
    class AssemblyDiversion : DiversionImpl<IAssemblyInfo>, IAssemblyDiversion
    {
        private readonly Lazy<IDiversions<ITypeInfo, ITypeDiversion>> _typeDiversions;
        private readonly Lazy<ICollectionDiversions<IAttributeInfo>> _attributeDiversions;
        private readonly Lazy<bool> _hasDiverged;

        public AssemblyDiversion(IAssemblyInfo old, IAssemblyInfo @new) : base(old, @new)
        {
            _typeDiversions = new Lazy<IDiversions<ITypeInfo, ITypeDiversion>>(() => DiversionDiviner.DivineDiversions(old.Types, @new.Types,
                (o, n) => new TypeDiversion(o, n)), true);
            _attributeDiversions = new Lazy<ICollectionDiversions<IAttributeInfo>>(() => DiversionDiviner.DivineCollectionDiversions(old.Attributes, @new.Attributes), true);
            _hasDiverged = new Lazy<bool>(() => base.HasDiverged() || TypeDiversions.HasDiverged() || AttributeDiversions.HasDiverged(), true);
        }

        public IDiversions<ITypeInfo, ITypeDiversion> TypeDiversions
        {
            get { return _typeDiversions.Value; }
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