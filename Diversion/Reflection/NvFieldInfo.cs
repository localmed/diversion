using System.Reflection;

namespace Diversion.Reflection
{
    internal class NvFieldInfo : NvMemberInfo, IFieldInfo
    {
        private readonly FieldInfo _member;

        public NvFieldInfo(FieldInfo member)
            : base(member)
        {
            _member = member;
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
            get { return new NvTypeInfo(_member.FieldType); }
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