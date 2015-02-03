using System.Diagnostics.Contracts;
using System.Reflection;

namespace Diversion.Reflection
{
    class NvFieldInfo : NvMemberInfo, IFieldInfo
    {
        private readonly FieldInfo _member;
        private readonly ITypeInfo _type;

        public NvFieldInfo(IReflectionInfoFactory reflectionInfoFactory, FieldInfo member)
            : base(reflectionInfoFactory, member)
        {
            Contract.Requires(reflectionInfoFactory != null);
            Contract.Requires(member != null);
            _member = member;
            _type = reflectionInfoFactory.FromReflection(_member.FieldType);
        }

        public override bool IsPublic
        {
            get { return _member.IsPublic; }
        }

        public override bool IsStatic
        {
            get { return _member.IsStatic; }
        }

        public ITypeInfo Type
        {
            get { return _type; }
        }

        public bool IsReadOnly
        {
            get { return _member.IsInitOnly; }
        }

        public bool IsConstant
        {
            get { return _member.IsLiteral; }
        }
    }
}