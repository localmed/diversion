using Diversion.Reflection;
using System;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using System.Diagnostics;

namespace Diversion.Cecil
{
    public class ReflectionInfoFactory : IReflectionInfoFactory
    {
        public ITypeReference GetReference(Mono.Cecil.TypeReference type)
        {
            return type is GenericParameter ? new GenericParameterInfo(this, type as GenericParameter) : new TypeReference(this, type);
        }

        public ITypeReference GetReference(InterfaceImplementation type)
        {
            return new TypeReference(this, type.InterfaceType);
        }

        public ITypeReference GetReference(TypeDefinition type)
        {
            return new TypeReference(this, type);
        }

        public IMemberInfo GetInfo(MethodDefinition member)
        {
            return (member.IsConstructor) ? (IMemberInfo)new ConstructorInfo(this, member) : new MethodInfo(this, member);
        }

        public IMemberInfo GetInfo(FieldDefinition member)
        {
            return new FieldInfo(this, member);
        }

        public IMemberInfo GetInfo(PropertyDefinition member)
        {
            return new PropertyInfo(this, member);
        }

        public IMemberInfo GetInfo(EventDefinition member)
        {
            return new EventInfo(this, member);
        }

        public ITypeInfo GetInfo(TypeDefinition type)
        {
            return new TypeInfo(this, type);
        }

        public IAttributeInfo GetInfo(CustomAttribute attribute)
        {
            return new AttributeInfo(this, attribute);
        }

        public IParameterInfo GetInfo(ParameterDefinition parameter)
        {
            return new ParameterInfo(this, parameter);
        }
    }

}
