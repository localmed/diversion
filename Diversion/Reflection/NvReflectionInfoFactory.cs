using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace Diversion.Reflection
{
    public class NvReflectionInfoFactory : IReflectionInfoFactory
    {
        public IAssemblyInfo FromFile(string assemblyPath)
        {
            byte[] md5 = MD5.Create().ComputeHash(File.ReadAllBytes(assemblyPath));
            Assembly assembly = Assembly.ReflectionOnlyLoadFrom(assemblyPath);
            foreach (var assemblyName in assembly.GetReferencedAssemblies())
            {
                try
                {
                    Assembly.ReflectionOnlyLoad(assemblyName.FullName);
                }
                catch (Exception)
                {
                    Assembly.ReflectionOnlyLoadFrom(Path.Combine(Path.GetDirectoryName(assemblyPath),
                        assemblyName.Name + ".dll"));
                }
            }
            return new NvAssemblyInfo(this, assembly, md5);
        }

        public IMemberInfo FromReflection(MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Constructor:
                    return new NvConstructorInfo(this, (ConstructorInfo)member);
                case MemberTypes.Event:
                    return new NvEventInfo(this, (EventInfo)member);
                case MemberTypes.Field:
                    return new NvFieldInfo(this, (FieldInfo)member);
                case MemberTypes.Method:
                    return new NvMethodInfo(this, (MethodInfo)member);
                case MemberTypes.Property:
                    return new NvPropertyInfo(this, (PropertyInfo)member);
                case MemberTypes.TypeInfo:
                case MemberTypes.NestedType:
                    return FromReflection((Type) member);
            }
            return null;
        }

        public ITypeInfo FromReflection(Type type)
        {
            return type.IsGenericParameter ? new NvGenericParameterInfo(this, type) : new NvTypeInfo(this, type);
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
