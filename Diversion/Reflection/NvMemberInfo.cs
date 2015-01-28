using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;

namespace Diversion.Reflection
{
    internal abstract class NvMemberInfo : IMemberInfo
    {
        private readonly MemberInfo _member;
        private readonly Lazy<IEnumerable<IAttributeInfo>> _attributes;

        protected NvMemberInfo(MemberInfo member)
        {
            _member = member;
            _attributes = new Lazy<IEnumerable<IAttributeInfo>>(() => _member.GetCustomAttributesData().Select(a => (IAttributeInfo)null).ToArray(), true);
        }

        public IEnumerable<IAttributeInfo> Attributes
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

        protected static IMemberInfo FromMemberInfo(MemberInfo member)
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
                    return new NvTypeInfo((Type)member);
            }
            return null;
        }
    }

    internal static class MemberInfoExtensions
    {
        public static bool IsPublicOrProtected(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Constructor:
                    return ((ConstructorInfo)member).IsPublicOrProtected();
                case MemberTypes.Event:
                    return ((EventInfo)member).IsPublicOrProtected();
                case MemberTypes.Field:
                    return ((FieldInfo)member).IsPublicOrProtected();
                case MemberTypes.Method:
                    return ((MethodInfo)member).IsPublicOrProtected();
                case MemberTypes.Property:
                    return ((PropertyInfo)member).IsPublicOrProtected();
                case MemberTypes.TypeInfo:
                case MemberTypes.NestedType:
                    return ((Type)member).IsPublicOrProtected();
            }
            return false;
        }

        private static bool IsPublicOrProtected(this MethodBase member)
        {
            return member.IsPublic || member.IsFamilyOrAssembly || member.IsFamily;
        }

        private static bool IsPublicOrProtected(this FieldInfo member)
        {
            return member.IsPublic || member.IsFamilyOrAssembly || member.IsFamily;
        }

        private static bool IsPublicOrProtected(this Type member)
        {
            return member.IsPublic || member.IsNestedFamily || member.IsNestedPublic || member.IsNestedFamORAssem;
        }

        private static bool IsPublicOrProtected(this PropertyInfo member)
        {
            return (member.GetMethod != null && member.GetMethod.IsPublicOrProtected()) ||
                   (member.SetMethod != null && member.SetMethod.IsPublicOrProtected());
        }

        private static bool IsPublicOrProtected(this EventInfo member)
        {
            return (member.AddMethod != null && member.AddMethod.IsPublicOrProtected()) ||
                   (member.RemoveMethod != null && member.RemoveMethod.IsPublicOrProtected()) ||
                   (member.RaiseMethod != null && member.RaiseMethod.IsPublicOrProtected());
        }
    }

}