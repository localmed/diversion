using System;
using Diversion.Reflection;

namespace Diversion
{
    class AssemblyDiversion : DiversionImpl<IAssemblyInfo>, IAssemblyDiversion
    {
        private readonly Lazy<IDiversions<ITypeInfo, ITypeDiversion>> _typeChanges;
        private readonly Lazy<bool> _hasDiverged;

        public AssemblyDiversion(IAssemblyInfo old, IAssemblyInfo @new) : base(old, @new)
        {
            _typeChanges = new Lazy<IDiversions<ITypeInfo, ITypeDiversion>>(() => DiversionDiviner.DivineDiversions(old.Types, @new.Types,
                (o, n) => new TypeDiversion(o, n)), true);
            _hasDiverged = new Lazy<bool>(() => base.HasDiverged() || TypeDiversions.HasDiverged(), true);
        }

        public IDiversions<ITypeInfo, ITypeDiversion> TypeDiversions
        {
            get { return _typeChanges.Value; }
        }

        public override bool HasDiverged()
        {
            return _hasDiverged.Value;
        }
    }
}