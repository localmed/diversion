using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Diversion.Reflection
{
    [Serializable]
    class NvTypeInfo : NvMemberInfo, ITypeInfo
    {
        private readonly bool _isPublic;
        private readonly bool _isStatic;

        public NvTypeInfo(IReflectionInfoFactory reflectionInfoFactory, Type type) : base(reflectionInfoFactory, type)
        {
            Base = type.BaseType == null ? null : reflectionInfoFactory.GetReference(type.BaseType);
            Interfaces = type.GetInterfaces().Select(reflectionInfoFactory.GetReference).OrderBy(i => i.Identity).ToArray();
            Members = type
                .GetMembers(BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic)
                .Where(m => m.IsPublicOrProtected()).Select(reflectionInfoFactory.GetInfo).OrderBy(i => i.Identity).ToArray();
            GenericArguments = type.GetGenericArguments().Select(reflectionInfoFactory.GetReference).ToArray();
            _isPublic = type.IsPublic;
            _isStatic = type.IsSealed && type.IsAbstract && type.IsClass;
            IsGenericType = type.IsGenericType;
            IsInterface = type.IsInterface;
            IsAbstract = type.IsAbstract;
            Namespace = type.Namespace;
            Name = type.IsGenericType && type.Name.Contains('`') ? string.Format("{0}<{1}>", type.Name.Substring(0, type.Name.IndexOf('`')), string.Join(",", type.GetGenericArguments().Select(t => t.IsGenericParameter ? string.Empty : reflectionInfoFactory.GetReference(t).Identity))) : type.Name;
        }

        public override bool IsPublic
        {
            get { return _isPublic; }
        }

        public override bool IsStatic
        {
            get { return _isStatic; }
        }

        public bool IsAbstract { get; private set; }

        public bool IsInterface { get; private set; }

        public string Namespace { get; private set; }

        public ITypeReference Base { get; private set; }

        public IReadOnlyList<ITypeReference> Interfaces { get; private set; }

        public IReadOnlyList<IMemberInfo> Members { get; private set; }

        public bool IsGenericType { get; private set; }

        public IReadOnlyList<ITypeReference> GenericArguments { get; private set; }

        public override string Identity
        {
            get { return DeclaringType == null ? string.Join(".", Namespace, Name) : string.Join("+", DeclaringType, Name); }
        }
    }
}