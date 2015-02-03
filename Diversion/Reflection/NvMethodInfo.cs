using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Diversion.Reflection
{
    internal class NvMethodInfo : NvMemberInfo, IMethodInfo
    {
        private readonly MethodInfo _method;
        private readonly Lazy<IReadOnlyList<IParameterInfo>> _parameters;
        private readonly Lazy<IReadOnlyList<IGenericParameterInfo>> _genericParameters;
        private readonly Lazy<IParameterInfo> _returnType;

        public NvMethodInfo(MethodInfo method) : base(method)
        {
            _method = method;
            _parameters = new Lazy<IReadOnlyList<IParameterInfo>>(_method.GetParameters().Select(p => (IParameterInfo)null).ToArray, true);
            _genericParameters = new Lazy<IReadOnlyList<IGenericParameterInfo>>(_method.GetGenericArguments().Select(a => (IGenericParameterInfo)FromMemberInfo(a)).ToArray, true);
            _returnType = new Lazy<IParameterInfo>(() => (IParameterInfo)_method.ReturnParameter, true);
        }

        public override bool IsPublic
        {
            get { return _method.IsPublic; }
        }

        public override bool IsStatic
        {
            get { return _method.IsStatic; }
        }

        public bool IsVirtual
        {
            get { return _method.IsVirtual; }
        }

        public bool IsAbstract
        {
            get { return _method.IsAbstract; }
        }

        public IReadOnlyList<IParameterInfo> Parameters
        {
            get { return _parameters.Value; }
        }

        public IReadOnlyList<IGenericParameterInfo> GenericParameters
        {
            get { return _genericParameters.Value; }
        }

        public IParameterInfo ReturnType
        {
            get { return _returnType.Value; }
        }

        public bool IsGenericMethod
        {
            get { return _method.IsGenericMethod; }
        }

        public override string Identity
        {
            get { return string.Format("{0}({1})", base.Identity, string.Join(",",Parameters.Select(p => p.Type))); }
        }
    }
}