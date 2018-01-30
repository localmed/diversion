using Diversion.Reflection;
using System;
using System.Reflection;

namespace Diversion.Cecil
{
    [Serializable]
    public class FieldInfo : MemberInfo, IFieldInfo
    {
        private readonly ITypeReference _type;
        private readonly bool _isPublic;
        private readonly bool _isStatic;
        private readonly bool _isReadOnly;
        private readonly bool _isConstant;
        private readonly bool _isOnApiSurface;
        private readonly object _constant;

        public FieldInfo(IReflectionInfoFactory reflectionInfoFactory, Mono.Cecil.FieldDefinition member)
            : base(reflectionInfoFactory, member)
        {
            _type = reflectionInfoFactory.GetReference(member.FieldType);
            _isPublic = member.IsPublic;
            _isStatic = member.IsStatic;
            _isReadOnly = member.IsInitOnly;
            _isConstant = member.IsLiteral;
            _isOnApiSurface = member.IsPublic || member.IsFamily || member.IsFamilyOrAssembly;
            _constant = member.Constant;
        }

        public override bool IsOnApiSurface => _isOnApiSurface;

        public override bool IsPublic => _isPublic;

        public override bool IsStatic => _isStatic;

        public ITypeReference Type => _type;

        public bool IsReadOnly => _isReadOnly;

        public bool IsConstant => _isConstant;

        public object Constant => _constant;
    }
}