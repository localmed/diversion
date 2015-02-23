using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Diversion.Reflection
{
    [Serializable]
    class NvConstructorInfo : NvMemberInfo, IConstructorInfo
    {
        private readonly IReadOnlyList<IParameterInfo> _parameters;
        private readonly bool _isPublic;
        private readonly bool _isStatic;

        public NvConstructorInfo(IReflectionInfoFactory reflectionInfoFactory, ConstructorInfo member)
            : base(reflectionInfoFactory, member)
        {
            _isPublic = member.IsPublic;
            _isStatic = member.IsStatic;
            _parameters = member.GetParameters().Select(reflectionInfoFactory.GetInfo).ToArray();
        }

        public override bool IsPublic
        {
            get { return _isPublic; }
        }

        public override bool IsStatic
        {
            get { return _isStatic; }
        }

        public IReadOnlyList<IParameterInfo> Parameters
        {
            get { return _parameters; }
        }

        public override string Identity
        {
            get { return string.Format("{0}.{1}({2})", DeclaringType, DeclaringType.Name, string.Join(",", Parameters.Select(p => p.Type))); }
        }
    }
}