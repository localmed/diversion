using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Diversion.Reflection
{
    internal abstract class NvMemberInfo : IMemberInfo
    {
        private readonly MemberInfo _member;
        private readonly Lazy<IReadOnlyList<IAttributeInfo>> _attributes;
        private readonly Lazy<ITypeInfo> _declaringType;

        protected NvMemberInfo(MemberInfo member)
        {
            _member = member;
            _declaringType = new Lazy<ITypeInfo>(() => (ITypeInfo)FromMemberInfo(_member.DeclaringType), true);
            _attributes = new Lazy<IReadOnlyList<IAttributeInfo>>(() => _member.GetCustomAttributesData().Select(a => (IAttributeInfo)null).ToArray(), true);
        }

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

        public static IMemberInfo FromMemberInfo(MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Constructor:
                    return new NvConstructorInfo((ConstructorInfo) member);
                case MemberTypes.Event:
                    return new NvEventInfo((EventInfo)member);
                case MemberTypes.Field:
                    return new NvFieldInfo((FieldInfo)member);
                case MemberTypes.Method:
                    return new NvMethodInfo((MethodInfo)member);
                case MemberTypes.Property:
                    return new NvPropertyInfo((PropertyInfo)member);
                case MemberTypes.TypeInfo:
                case MemberTypes.NestedType:
                    return ((Type)member).IsGenericParameter ? new NvGenericParameterInfo((Type)member) : new NvTypeInfo((Type)member);
            }
            return null;
        }

        public sealed override string ToString()
        {
            return Identity;
        }
    }
}