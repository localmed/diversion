using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace Diversion.Reflection
{
    class NvConstructorInfo : NvMemberInfo, IConstructorInfo
    {
        private readonly ConstructorInfo _member;
        private readonly Lazy<IReadOnlyList<IParameterInfo>> _parameters;

        public NvConstructorInfo(IReflectionInfoFactory reflectionInfoFactory, ConstructorInfo member)
            : base(reflectionInfoFactory, member)
        {
            Contract.Requires(reflectionInfoFactory != null);
            Contract.Requires(member != null);
            _member = member;
            _parameters = new Lazy<IReadOnlyList<IParameterInfo>>(_member.GetParameters().Select(reflectionInfoFactory.FromReflection).ToArray, true);
        }

        public override bool IsPublic
        {
            get { return _member.IsPublic; }
        }

        public override bool IsStatic
        {
            get { return _member.IsStatic; }
        }

        public IReadOnlyList<IParameterInfo> Parameters
        {
            get { return _parameters.Value; }
        }

        public override string Identity
        {
            get { return string.Format("{0}.{1}({2})", DeclaringType, DeclaringType.Name, string.Join(",", Parameters.Select(p => p.Type))); }
        }
    }
}