using System;
using System.Linq;
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

        public static Type GetBaseDeclaringType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).GetBaseDeclaringType();
                case MemberTypes.Method:
                    return ((MethodInfo)member).GetBaseDeclaringType();
                case MemberTypes.Property:
                    return ((PropertyInfo)member).GetBaseDeclaringType();
            }
            return member.DeclaringType;
        }

        public static Type GetBaseDeclaringType(this EventInfo it)
        {
            return (it.GetAddMethod(true) ?? it.GetRemoveMethod(true)).GetBaseDefinition()?.DeclaringType ?? it.DeclaringType;
        }

        public static Type GetBaseDeclaringType(this PropertyInfo it)
        {
            return (it.GetGetMethod(true) ?? it.GetSetMethod(true)).GetBaseDefinition()?.DeclaringType ?? it.DeclaringType;
        }

        public static Type GetBaseDeclaringType(this MethodInfo it)
        {
            return it.GetBaseDefinition().DeclaringType;
        }

        public static bool IsOverride(this MethodInfo method)
        {
            return method.GetBaseDeclaringType() != method.DeclaringType;
        }

        public static bool IsOverride(this PropertyInfo property)
        {
            return property.GetBaseDeclaringType() != property.DeclaringType;
        }

        public static bool IsOverride(this EventInfo it)
        {
            return it.GetBaseDeclaringType() != it.DeclaringType;
        }

        public static PropertyInfo GetBaseDefinition(this PropertyInfo property)
        {
            var key = (property.GetGetMethod(true) ?? property.GetSetMethod(true)).GetBaseDefinition();

            return key.DeclaringType == property.DeclaringType ? property :
                key.DeclaringType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .First(p => (p.GetGetMethod(true) ?? p.GetSetMethod(true)) == key);
        }


        public static EventInfo GetBaseDefinition(this EventInfo property)
        {
            var key = (property.GetAddMethod(true) ?? property.GetRemoveMethod(true)).GetBaseDefinition();

            return key.DeclaringType == property.DeclaringType ? property :
                key.DeclaringType.GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .First(p => (p.GetAddMethod(true) ?? p.GetRemoveMethod(true)) == key);
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