using Diversion.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Diversion.Cecil
{
    [Serializable]
    public class MethodInfo : MemberInfo, IMethodInfo
    {
        private readonly IReadOnlyList<IParameterInfo> _parameters;
        private readonly IReadOnlyList<ITypeReference> _genericArguments;
        private readonly ITypeReference _returnType;
        private readonly bool _isPublic;
        private readonly bool _isStatic;
        private readonly bool _isVirtual;
        private readonly bool _isAbstract;
        private readonly bool _isGenericMethod;
        private readonly bool _isOnPublicApiSurface;
        private readonly byte[] _implementation;

        public MethodInfo(IReflectionInfoFactory reflectionInfoFactory, Mono.Cecil.MethodDefinition method) : base(reflectionInfoFactory, method)
        {
            _isPublic = method.IsPublic;
            _isStatic = method.IsStatic;
            _isVirtual = method.IsVirtual;
            _isAbstract = method.IsAbstract;
            _isGenericMethod = method.IsGenericInstance;
            _parameters = method.Parameters.Select(reflectionInfoFactory.GetInfo).ToArray();
            _genericArguments = method.GenericParameters.Select(reflectionInfoFactory.GetReference).ToArray();
            _returnType = method.ReturnType != null ? reflectionInfoFactory.GetReference(method.ReturnType) : null;
            _implementation = new byte[0];
            _isOnPublicApiSurface = method.IsPublic || method.IsFamilyOrAssembly || method.IsFamily;
        }

        public override bool IsOnApiSurface => _isOnPublicApiSurface;

        public override bool IsPublic => _isPublic;

        public override bool IsStatic => _isStatic;

        public bool IsVirtual => _isVirtual;

        public bool IsAbstract => _isAbstract;

        public IReadOnlyList<IParameterInfo> Parameters => _parameters;

        public IReadOnlyList<ITypeReference> GenericArguments => _genericArguments;

        public ITypeReference ReturnType => _returnType;

        public bool IsGenericMethod => _isGenericMethod;

        public override string Identity
        {
            get { return string.Format("{0}({1})", base.Identity, string.Join(",", Parameters.Select(p => p.Type))); }
        }

        public override byte[] Implementation => _implementation;
    }
}