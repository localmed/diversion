using System;
using System.Reflection;

namespace Diversion.Reflection
{
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