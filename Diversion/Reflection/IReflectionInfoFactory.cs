using System;
using System.Reflection;

namespace Diversion.Reflection
{
    interface IReflectionInfoFactory
    {
        ITypeReference GetReference(Type type);
        ITypeInfo GetInfo(Type type);
        IMemberInfo GetInfo(MemberInfo member);
        IAttributeInfo GetInfo(CustomAttributeData attribute);
        IParameterInfo GetInfo(ParameterInfo parameter);
    }
}