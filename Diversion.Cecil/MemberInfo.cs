using Diversion.Reflection;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diversion.Cecil
{
    [Serializable]
    public abstract class MemberInfo : IMemberInfo
    {
        private readonly IReadOnlyList<IAttributeInfo> _attributes;
        private readonly ITypeReference _declaringType;
        private readonly ITypeReference _baseDeclaringType;

        protected MemberInfo(IReflectionInfoFactory reflectionInfoFactory, IMemberDefinition member)
        {
            var baseDeclaringType = member.GetBaseDeclaringType();
            _baseDeclaringType = baseDeclaringType == null ? null : reflectionInfoFactory.GetReference(baseDeclaringType);
            _declaringType = member.DeclaringType == null ? null : reflectionInfoFactory.GetReference(member.DeclaringType);
            _attributes = member.CustomAttributes.Select(reflectionInfoFactory.GetInfo).ToArray();
            Name = member.Name;
        }

        public ITypeReference BaseDeclaringType
        {
            get { return _baseDeclaringType; }
        }

        public ITypeReference DeclaringType
        {
            get { return _declaringType; }
        }

        public IReadOnlyList<IAttributeInfo> Attributes
        {
            get { return _attributes; }
        }

        public string Name { get; protected set; }

        public abstract bool IsPublic { get; }

        public abstract bool IsStatic { get; }

        public abstract bool IsOnApiSurface { get; }

        public override bool Equals(object obj)
        {
            var other = obj as MemberInfo;
            return other != null && GetType() == other.GetType() && Identity == other.Identity;
        }

        public override int GetHashCode()
        {
            return (GetType() + Identity).GetHashCode();
        }

        public virtual string Identity
        {
            get { return string.Join(".", BaseDeclaringType, Name); }
        }

        public sealed override string ToString()
        {
            return Identity;
        }

        public virtual byte[] Implementation { get { return new byte[0]; } }
    }
}