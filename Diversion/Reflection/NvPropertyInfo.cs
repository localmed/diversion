using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Diversion.Reflection
{
    internal class NvPropertyInfo : NvMemberInfo, IPropertyInfo
    {
        private readonly PropertyInfo _member;
        private readonly Lazy<IReadOnlyList<IParameterInfo>> _parameters;

        public NvPropertyInfo(PropertyInfo member)
            : base(member)
        {
            _member = member;
            _parameters = new Lazy<IReadOnlyList<IParameterInfo>>(_member.GetIndexParameters().Select(p => (IParameterInfo)null).ToArray, true);
        }

        public override bool IsPublic
        {
            get { return (_member.GetMethod ?? _member.SetMethod).IsPublic || (_member.SetMethod ?? _member.GetMethod).IsPublic; }
        }

        public override bool IsStatic
        {
            get { return (_member.GetMethod ?? _member.SetMethod).IsStatic; }
        }

        public bool IsVirtual
        {
            get { return (_member.GetMethod ?? _member.SetMethod).IsVirtual; }
        }

        public bool IsAbstract
        {
            get { return (_member.GetMethod ?? _member.SetMethod).IsAbstract; }
        }

        public IReadOnlyList<IParameterInfo> IndexerParameters
        {
            get { return _parameters.Value; }
        }

        public ITypeInfo Type
        {
            get { return new NvTypeInfo(_member.PropertyType); }
        }

        public override string Identity
        {
            get
            {
                return IndexerParameters.Any()
                    ? string.Format("{0}[{1}]", base.Identity, string.Join(",", IndexerParameters.Select(p => p.Type)))
                    : base.Identity;
            }
        }
    }
}