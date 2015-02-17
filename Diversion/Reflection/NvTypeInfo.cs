using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace Diversion.Reflection
{
    [Serializable]
    public class NvTypeInfo : NvMemberInfo, ITypeInfo
    {
        private readonly bool _isPublic;
        private readonly bool _isStatic;

        public NvTypeInfo(IReflectionInfoFactory reflectionInfoFactory, Type type) : base(reflectionInfoFactory, type)
        {
            Contract.Requires(reflectionInfoFactory != null);
            Contract.Requires(type != null);
            Base = type.BaseType == null ? null : reflectionInfoFactory.GetReference(type.BaseType);
            Interfaces = type.GetInterfaces().Select(reflectionInfoFactory.GetReference).ToArray();
            Members = type
                .GetMembers(BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic)
                .Where(m => m.IsPublicOrProtected()).Select(reflectionInfoFactory.FromReflection).ToArray();
            GenericArguments = type.GetGenericArguments().Select(reflectionInfoFactory.GetReference).ToArray();
            _isPublic = type.IsPublic;
            _isStatic = type.IsSealed && type.IsAbstract && type.IsClass;
            IsInterface = type.IsInterface;
            IsAbstract = type.IsAbstract;
            Namespace = type.Namespace;
            IsGenericType = type.IsGenericType;
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