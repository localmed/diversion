using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Diversion.Reflection
{
    [Serializable]
    class NvPropertyInfo : NvMemberInfo, IPropertyInfo
    {
        private readonly bool _isPublic;
        private readonly bool _isStatic;
        private readonly bool _isVirtual;
        private readonly bool _isAbstract;
        private readonly bool _isOnApiSurface;

        public NvPropertyInfo(IReflectionInfoFactory reflectionInfoFactory, PropertyInfo member)
            : base(reflectionInfoFactory, member)
        {
            IndexerParameters = member.GetIndexParameters().Select(reflectionInfoFactory.GetInfo).ToArray();
            Type = reflectionInfoFactory.GetReference(member.PropertyType);
            _isPublic = (member.GetMethod ?? member.SetMethod).IsPublic || (member.SetMethod ?? member.GetMethod).IsPublic;
            _isStatic = (member.GetMethod ?? member.SetMethod).IsStatic;
            _isVirtual = (member.GetMethod ?? member.SetMethod).IsVirtual;
            _isAbstract = (member.GetMethod ?? member.SetMethod).IsAbstract;
            _isOnApiSurface = member.IsPublicOrProtected();
        }

        public override bool IsOnApiSurface => _isOnApiSurface;

        public override bool IsPublic => _isPublic;

        public override bool IsStatic => _isStatic;

        public bool IsVirtual => _isVirtual;

        public bool IsAbstract => _isAbstract;

        public IReadOnlyList<IParameterInfo> IndexerParameters { get; private set; }

        public ITypeReference Type { get; private set; }

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