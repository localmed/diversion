using System;
using System.Reflection;

namespace Diversion.Reflection
{
    [Serializable]
    public class NvFieldInfo : NvMemberInfo, IFieldInfo
    {
        private readonly ITypeReference _type;
        private readonly bool _isPublic;
        private readonly bool _isStatic;
        private readonly bool _isReadOnly;
        private readonly bool _isConstant;

        public NvFieldInfo(IReflectionInfoFactory reflectionInfoFactory, FieldInfo member)
            : base(reflectionInfoFactory, member)
        {
            _type = reflectionInfoFactory.GetReference(member.FieldType);
            _isPublic = member.IsPublic;
            _isStatic = member.IsStatic;
            _isReadOnly = member.IsInitOnly;
            _isConstant = member.IsLiteral;
        }

        public override bool IsPublic
        {
            get { return _isPublic; }
        }

        public override bool IsStatic
        {
            get { return _isStatic; }
        }

        public ITypeReference Type
        {
            get { return _type; }
        }

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
        }

        public bool IsConstant
        {
            get { return _isConstant; }
        }
    }
}