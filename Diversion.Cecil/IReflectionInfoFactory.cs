using Mono.Cecil;
using Diversion.Reflection;

namespace Diversion.Cecil
{
    public interface IReflectionInfoFactory
    {
        ITypeReference GetReference(Mono.Cecil.TypeReference type);
        ITypeReference GetReference(TypeDefinition type);
        IMemberInfo GetInfo(MethodDefinition member);
        IMemberInfo GetInfo(PropertyDefinition member);
        IMemberInfo GetInfo(EventDefinition member);
        IMemberInfo GetInfo(FieldDefinition member);
        ITypeInfo GetInfo(TypeDefinition type);
        IAttributeInfo GetInfo(CustomAttribute attribute);
        IParameterInfo GetInfo(ParameterDefinition parameter);
    }
}
