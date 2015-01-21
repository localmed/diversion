using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Diversion
{
    internal class AssemblyInfoFactory
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
            Version version = assembly.CustomAttributes.Where(attr => attr.AttributeType == typeof(AssemblyInformationalVersionAttribute))
                .Select(attr => new Version((string)attr.ConstructorArguments[0].Value)).FirstOrDefault() ?? assembly.GetName().Version;
            Version frameworkVersion = assembly.CustomAttributes.Where(attr => attr.AttributeType == typeof(TargetFrameworkAttribute))
                .Select(attr => new Version(Regex.Match((string)attr.ConstructorArguments[0].Value, @"\.NETFramework,Version=v(.*)").Result("$1"))).FirstOrDefault() ?? new Version(assembly.ImageRuntimeVersion);

            return new NvAssemblyInfo(assembly.FullName, version, frameworkVersion, md5, assembly.GetExportedTypes().AsParallel().Select(ToTypeInfo));
        }

        private ITypeInfo ToTypeInfo(Type type)
        {
            return new NvTypeInfo(type);
            //return new NvTypeInfo(type.FullName, type.GetInterfaces(), type.GetMembers(BindingFlags.Public).AsParallel().Select(ToMemberInfo));
        }
    }
}