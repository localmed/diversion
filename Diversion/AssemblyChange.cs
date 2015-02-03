using System;
using Diversion.Reflection;

namespace Diversion
{
    class AssemblyChange : ChangeImpl<IAssemblyInfo>, IAssemblyChange
    {
        private readonly Lazy<IChanges<ITypeInfo, ITypeChange>> _typeChanges;

        public AssemblyChange(IAssemblyInfo old, IAssemblyInfo @new) : base(old, @new)
        {
            _typeChanges = new Lazy<IChanges<ITypeInfo, ITypeChange>>(() => ChangeDiviner.DivineChanges(old.Types, @new.Types,
                (o, n) => new TypeChange(o, n)), true);
        }

        public IChanges<ITypeInfo, ITypeChange> TypeChanges
        {
            get { return _typeChanges.Value; }
        }
    }
}