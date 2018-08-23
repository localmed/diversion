using Diversion.Reflection;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Diversion.Cecil
{
    public class ConstructorInfo : MemberInfo, IConstructorInfo
    {
        private readonly bool _isOnApiSurface;
        private readonly bool _isPublic;
        private readonly bool _isStatic;

        public ConstructorInfo(IReflectionInfoFactory reflectionInfoFactory, MethodDefinition member)
            : base(reflectionInfoFactory, member)
        {
            Parameters = member.Parameters.Select(reflectionInfoFactory.GetInfo).ToArray();
            _isPublic = member.IsPublic;
            _isStatic = member.IsStatic;
            _isOnApiSurface = member.IsPublic || member.IsFamily || member.IsFamilyOrAssembly;
        }

        public override bool IsOnApiSurface => _isOnApiSurface;

        public override bool IsPublic => _isPublic;

        public override bool IsStatic => _isStatic;

        public IReadOnlyList<IParameterInfo> Parameters { get; }

        public override string Identity
        {
            get { return string.Format("{0}.{1}({2})", BaseDeclaringType, BaseDeclaringType.Name, string.Join(",", Parameters.Select(p => p.Type))); }
        }
    }
}