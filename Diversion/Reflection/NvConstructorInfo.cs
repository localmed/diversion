using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Diversion.Reflection
{
    internal class NvConstructorInfo : NvMemberInfo, IConstructorInfo
    {
        private readonly ConstructorInfo _member;
        private readonly Lazy<IEnumerable<IParameterInfo>> _parameters;

        public NvConstructorInfo(ConstructorInfo member)
            : base(member)
        {
            _member = member;
            _parameters = new Lazy<IEnumerable<IParameterInfo>>(() => _member.GetParameters().Select(p => (IParameterInfo)null).ToArray(), true);
        }

        public override bool IsPublic
        {
            get { return _member.IsPublic; }
        }

        public override bool IsStatic
        {
            get { return _member.IsStatic; }
        }

        public IEnumerable<IParameterInfo> Parameters
        {
            get { return _parameters.Value; }
        }
    }
}