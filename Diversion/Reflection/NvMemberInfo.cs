using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace Diversion.Reflection
{
    abstract class NvMemberInfo : IMemberInfo
    {
        private readonly MemberInfo _member;
        private readonly Lazy<IReadOnlyList<IAttributeInfo>> _attributes;
        private readonly Lazy<ITypeInfo> _declaringType;

        protected NvMemberInfo(IReflectionInfoFactory reflectionInfoFactory, MemberInfo member)
        {
            Contract.Requires(reflectionInfoFactory != null);
            Contract.Requires(member != null);
            _member = member;
            _declaringType = new Lazy<ITypeInfo>(() => _member.DeclaringType == null ? null : reflectionInfoFactory.FromReflection(_member.DeclaringType), true);
            _attributes = new Lazy<IReadOnlyList<IAttributeInfo>>(_member.GetCustomAttributesData().Select(reflectionInfoFactory.FromReflection).ToArray, true);
        }

        public MemberInfo Member { get { return _member; } }

        public ITypeInfo DeclaringType
        {
            get { return _declaringType.Value; }
        }

        public IReadOnlyList<IAttributeInfo> Attributes
        {
            get { return _attributes.Value; }
        }

        public string Name
        {
            get { return _member.Name; }
        }

        public abstract bool IsPublic { get; }

        public abstract bool IsStatic { get; }

        public override bool Equals(object obj)
        {
            var other = obj as NvMemberInfo;
            return other != null && _member == other._member;
        }

        public override int GetHashCode()
        {
            return _member.GetHashCode();
        }

        public virtual string Identity
        {
            get { return string.Join(".", DeclaringType, Name); }
        }

        public sealed override string ToString()
        {
            return Identity;
        }
    }
}