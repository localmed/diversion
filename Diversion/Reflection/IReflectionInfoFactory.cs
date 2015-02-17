using System;
using System.Reflection;

namespace Diversion.Reflection
{
    public interface IReflectionInfoFactory
    {
        IAssemblyInfo FromFile(string assemblyPath);
        ITypeReference GetReference(Type type);
        ITypeInfo GetInfo(Type type);
        IMemberInfo GetInfo(MemberInfo member);
        IAttributeInfo GetInfo(CustomAttributeData attribute);
        IParameterInfo GetInfo(ParameterInfo parameter);
    }
}