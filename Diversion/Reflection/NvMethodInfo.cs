using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Diversion.Reflection
{
    [Serializable]
    class NvMethodInfo : NvMemberInfo, IMethodInfo
    {
        private readonly IReadOnlyList<IParameterInfo> _parameters;
        private readonly IReadOnlyList<ITypeReference> _genericArguments;
        private readonly IParameterInfo _returnType;
        private readonly bool _isPublic;
        private readonly bool _isStatic;
        private readonly bool _isVirtual;
        private readonly bool _isAbstract;
        private readonly bool _isGenericMethod;

        public NvMethodInfo(IReflectionInfoFactory reflectionInfoFactory, MethodInfo method) : base(reflectionInfoFactory, method)
        {
            _isPublic = method.IsPublic;
            _isStatic = method.IsStatic;
            _isVirtual = method.IsVirtual;
            _isAbstract = method.IsAbstract;
            _isGenericMethod = method.IsGenericMethod;
            _parameters = method.GetParameters().Select(reflectionInfoFactory.GetInfo).ToArray();
            _genericArguments = method.GetGenericArguments().Select(reflectionInfoFactory.GetReference).ToArray();
            _returnType = reflectionInfoFactory.GetInfo(method.ReturnParameter);
        }

        public override bool IsPublic
        {
            get { return _isPublic; }
        }

        public override bool IsStatic
        {
            get { return _isStatic; }
        }

        public bool IsVirtual
        {
            get { return _isVirtual; }
        }

        public bool IsAbstract
        {
            get { return _isAbstract; }
        }

        public IReadOnlyList<IParameterInfo> Parameters
        {
            get { return _parameters; }
        }

        public IReadOnlyList<ITypeReference> GenericArguments
        {
            get { return _genericArguments; }
        }

        public IParameterInfo ReturnType
        {
            get { return _returnType; }
        }

        public bool IsGenericMethod
        {
            get { return _isGenericMethod; }
        }

        public override string Identity
        {
            get { return string.Format("{0}({1})", base.Identity, string.Join(",", Parameters.Select(p => p.Type))); }
        }
    }
}