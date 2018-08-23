using Diversion.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diversion.Cecil
{
    public class PropertyInfo : MemberInfo, IPropertyInfo
    {
        private readonly bool _isPublic;
        private readonly bool _isStatic;
        private readonly bool _isOnApiSurface;

        public PropertyInfo(IReflectionInfoFactory reflectionInfoFactory, Mono.Cecil.PropertyDefinition member)
            : base(reflectionInfoFactory, member)
        {
            IndexerParameters = member.Parameters.Select(reflectionInfoFactory.GetInfo).ToArray();
            Type = reflectionInfoFactory.GetReference(member.PropertyType);
            var p = member.GetMethod ?? member.SetMethod;
            var s = member.SetMethod ?? member.GetMethod;
            _isPublic = p.IsPublic || s.IsPublic;
            _isStatic = p.IsStatic;
            IsVirtual = p.IsVirtual;
            IsAbstract = p.IsAbstract;
            _isOnApiSurface = IsPublic || p.IsFamilyOrAssembly || p.IsFamily || s.IsFamilyOrAssembly || s.IsFamily;
        }

        public override bool IsOnApiSurface => _isOnApiSurface;

        public override bool IsPublic => _isPublic;

        public override bool IsStatic => _isStatic;

        public bool IsVirtual { get; }

        public bool IsAbstract { get; }

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