using Diversion.Reflection;

namespace Diversion.Cecil
{
    public class EventInfo : MemberInfo, IEventInfo
    {
        private readonly bool _isPublic;
        private readonly bool _isStatic;
        private readonly bool _isOnApiSurface;

        public EventInfo(IReflectionInfoFactory reflectionInfoFactory, Mono.Cecil.EventDefinition member)
            : base(reflectionInfoFactory, member)
        {
            EventHandlerType = reflectionInfoFactory.GetReference(member.EventType);
            IsAbstract = (member.AddMethod ?? member.RemoveMethod).IsAbstract;
            _isPublic = (member.AddMethod ?? member.RemoveMethod).IsPublic || (member.RemoveMethod ?? member.AddMethod).IsPublic;
            _isStatic = (member.AddMethod ?? member.RemoveMethod).IsStatic;
            IsVirtual = (member.AddMethod ?? member.RemoveMethod).IsVirtual;
            _isOnApiSurface = IsPublic;
        }

        public override bool IsOnApiSurface => _isOnApiSurface;

        public override bool IsPublic
        {
            get { return _isPublic; }
        }

        public override bool IsStatic
        {
            get { return _isStatic; }
        }

        public bool IsVirtual { get; }

        public bool IsAbstract { get; }

        public ITypeReference EventHandlerType { get; }
    }
}