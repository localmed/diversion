using System;
using System.Reflection;

namespace Diversion.Reflection
{
    class NvReflectionInfoFactory : IReflectionInfoFactory
    {
        public ITypeReference GetReference(Type type)
        {
            return type.IsGenericParameter ? new NvGenericParameterInfo(this, type) : new NvTypeReference(this, type);
        }

        public IMemberInfo GetInfo(MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Constructor:
                    return new NvConstructorInfo(this, (ConstructorInfo) member);
                case MemberTypes.Event:
                    return new NvEventInfo(this, (EventInfo) member);
                case MemberTypes.Field:
                    return new NvFieldInfo(this, (FieldInfo) member);
                case MemberTypes.Method:
                    return new NvMethodInfo(this, (MethodInfo) member);
                case MemberTypes.Property:
                    return new NvPropertyInfo(this, (PropertyInfo) member);
                case MemberTypes.TypeInfo:
                case MemberTypes.NestedType:
                    return GetInfo((Type) member);
            }
            return null;
        }

        public ITypeInfo GetInfo(Type type)
        {
            return new NvTypeInfo(this, type);
        }

        public IAttributeInfo GetInfo(CustomAttributeData attribute)
        {
            return new NvAttributeInfo(this, attribute);
        }

        public IParameterInfo GetInfo(ParameterInfo parameter)
        {
            return new NvParameterInfo(this, parameter);
        }
    }
}
