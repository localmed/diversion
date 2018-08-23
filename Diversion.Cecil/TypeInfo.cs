using Diversion.Reflection;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Diversion.Cecil
{
    public class TypeInfo : MemberInfo, ITypeInfo
    {
        private readonly bool _isPublic;
        private readonly bool _isStatic;
        private readonly bool _isOnApiSurface;

        public TypeInfo(IReflectionInfoFactory reflectionInfoFactory, TypeDefinition type) : base(reflectionInfoFactory, type)
        {
            Base = type.BaseType == null ? null : reflectionInfoFactory.GetReference(type.BaseType);
            Interfaces = type.Interfaces.Select(reflectionInfoFactory.GetReference).OrderBy(i => i.Identity).ToArray();
            Members = type.Methods.Select(reflectionInfoFactory.GetInfo)
                .Concat(type.Properties.Select(reflectionInfoFactory.GetInfo))
                .Concat(type.Fields.Select(reflectionInfoFactory.GetInfo))
                .Concat(type.NestedTypes.Select(reflectionInfoFactory.GetInfo))
                .Concat(type.Events.Select(reflectionInfoFactory.GetInfo))
                .Where(m => m.IsOnApiSurface)
                .OrderBy(i => i.Identity).ToArray();
            GenericArguments = type.GenericParameters.Select(reflectionInfoFactory.GetReference).ToArray();
            _isPublic = type.IsPublic;
            _isStatic = type.IsSealed && type.IsAbstract && type.IsClass;
            _isOnApiSurface = type.IsPublic || type.IsNestedFamily || type.IsNestedPublic || type.IsNestedFamilyOrAssembly;
            IsGenericType = type.IsGenericInstance;
            IsInterface = type.IsInterface;
            IsAbstract = type.IsAbstract;
            IsArray = type.IsArray;
            Namespace = type.Namespace;
            Name = type.Name.Contains('`') ? string.Format("{0}<{1}>", type.Name.Substring(0, type.Name.IndexOf('`')), string.Join(",", type.GenericParameters.Select(t => t.IsGenericParameter ? string.Empty : reflectionInfoFactory.GetReference(t).Identity))) : type.Name;
        }

        public override bool IsPublic => _isPublic;

        public override bool IsStatic => _isStatic;

        public override bool IsOnApiSurface => _isOnApiSurface;

        public bool IsArray { get; }

        public bool IsAbstract { get; }

        public bool IsInterface { get; }

        public string Namespace { get; }

        public ITypeReference Base { get; }

        public IReadOnlyList<ITypeReference> Interfaces { get; }

        public IReadOnlyList<IMemberInfo> Members { get; }

        public bool IsGenericType { get; }

        public IReadOnlyList<ITypeReference> GenericArguments { get; }

        public override string Identity
        {
            get { return BaseDeclaringType == null ? string.Join(".", Namespace, Name) : string.Join("+", BaseDeclaringType, Name); }
        }
    }
}