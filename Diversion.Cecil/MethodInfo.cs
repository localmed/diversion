using Diversion.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Diversion.Cecil
{
    public class MethodInfo : MemberInfo, IMethodInfo
    {
        private readonly bool _isOnPublicApiSurface;
        private readonly bool _isPublic;
        private readonly bool _isStatic;

        public MethodInfo(IReflectionInfoFactory reflectionInfoFactory, Mono.Cecil.MethodDefinition method) : base(reflectionInfoFactory, method)
        {
            _isPublic = method.IsPublic;
            _isStatic = method.IsStatic;
            IsVirtual = method.IsVirtual;
            IsAbstract = method.IsAbstract;
            IsGenericMethod = method.IsGenericInstance;
            Parameters = method.Parameters.Select(reflectionInfoFactory.GetInfo).ToArray();
            GenericArguments = method.GenericParameters.Select(reflectionInfoFactory.GetReference).ToArray();
            ReturnType = method.ReturnType != null ? reflectionInfoFactory.GetReference(method.ReturnType) : null;
            _isOnPublicApiSurface = method.IsPublic || method.IsFamilyOrAssembly || method.IsFamily;
        }

        public override bool IsOnApiSurface => _isOnPublicApiSurface;

        public override bool IsPublic => _isPublic;

        public override bool IsStatic => _isStatic;

        public bool IsVirtual { get; }

        public bool IsAbstract { get; }

        public IReadOnlyList<IParameterInfo> Parameters { get; }

        public IReadOnlyList<ITypeReference> GenericArguments { get; }

        public ITypeReference ReturnType { get; }

        public bool IsGenericMethod { get; }

        public override string Identity
        {
            get { return string.Format("{0}({1})", base.Identity, string.Join(",", Parameters.Select(p => p.Type))); }
        }
    }
}