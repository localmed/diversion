using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace Diversion.Reflection
{
    [Serializable]
    public class NvReflectionInfoFactory : IReflectionInfoFactory
    {
        public IAssemblyInfo FromFile(string assemblyPath)
        {
            byte[] md5 = MD5.Create().ComputeHash(File.ReadAllBytes(assemblyPath));
            using (var context = new AppDomainContext(new FileInfo(assemblyPath).DirectoryName))
            {
                return (IAssemblyInfo)context.Domain.CreateInstanceFromAndUnwrap(Assembly.GetExecutingAssembly().Location,
                    typeof (NvAssemblyInfo).FullName, false, BindingFlags.Default, null, new object[]{assemblyPath, md5}, null, null);
            }
        }

        public ITypeReference GetReference(Type type)
        {
            return type.IsGenericParameter ? new NvGenericParameterInfo(this, type) : new NvTypeReference(this, type);
        }

        public IMemberInfo FromReflection(MemberInfo member)
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
                    return FromReflection((Type) member);
            }
            return null;
        }

        public ITypeInfo FromReflection(Type type)
        {
            return new NvTypeInfo(this, type);
        }

        public IAttributeInfo FromReflection(CustomAttributeData attribute)
        {
            return new NvAttributeInfo(this, attribute);
        }

        public IParameterInfo FromReflection(ParameterInfo parameter)
        {
            return new NvParameterInfo(this, parameter);
        }
    }
}
