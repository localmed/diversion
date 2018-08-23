using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Diversion.Reflection
{
    [Serializable]
    abstract class NvMemberInfo : IMemberInfo
    {
        private readonly IReadOnlyList<IAttributeInfo> _attributes;
        private readonly ITypeReference _declaringType;
        private readonly ITypeReference _baseDeclaringType;

        protected NvMemberInfo(IReflectionInfoFactory reflectionInfoFactory, MemberInfo member)
        {
            _baseDeclaringType = member.GetBaseDeclaringType() == null ? null : reflectionInfoFactory.GetReference(member.GetBaseDeclaringType());
            _declaringType = member.DeclaringType == null ? null : reflectionInfoFactory.GetReference(member.DeclaringType);
            _attributes = member.GetCustomAttributesData().Select(reflectionInfoFactory.GetInfo).ToArray();
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
            var other = obj as NvMemberInfo;
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
    }
}