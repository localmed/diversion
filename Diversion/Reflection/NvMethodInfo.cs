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
        private readonly Lazy<IReadOnlyCollection<IParameterInfo>> _parameters;
        private readonly Lazy<IReadOnlyCollection<IGenericParameterInfo>> _genericParameters;
        private readonly Lazy<IParameterInfo> _returnType;

        public NvMethodInfo(MethodInfo method) : base(method)
        {
            _method = method;
            _parameters = new Lazy<IReadOnlyCollection<IParameterInfo>>(() => new ReadOnlyCollection<IParameterInfo>(_method.GetParameters().Select(p => (IParameterInfo)null).ToArray()), true);
            _genericParameters = new Lazy<IReadOnlyCollection<IGenericParameterInfo>>(() => new ReadOnlyCollection<IGenericParameterInfo>(_method.GetGenericArguments().Select(a => (IGenericParameterInfo)null).ToArray()), true);
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

        public IEnumerable<IParameterInfo> Parameters
        {
            get { return _parameters.Value; }
        }

        public IEnumerable<IGenericParameterInfo> GenericParameters
        {
            get { return _genericParameters.Value; }
        }

        public IParameterInfo ReturnType
        {
            get { return _returnType.Value; }
        }
    }
}