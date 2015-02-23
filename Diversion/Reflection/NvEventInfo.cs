using System;
using System.Reflection;

namespace Diversion.Reflection
{
    [Serializable]
    class NvEventInfo : NvMemberInfo, IEventInfo
    {
        private readonly ITypeReference _eventHandlerType;
        private readonly bool _isPublic;
        private readonly bool _isStatic;
        private readonly bool _isVirtual;
        private readonly bool _isAbstract;

        public NvEventInfo(IReflectionInfoFactory reflectionInfoFactory, EventInfo member)
            : base(reflectionInfoFactory, member)
        {
            _eventHandlerType = reflectionInfoFactory.GetReference(member.EventHandlerType);
            _isAbstract = (member.AddMethod ?? member.RemoveMethod).IsAbstract;
            _isPublic = (member.AddMethod ?? member.RemoveMethod).IsPublic || (member.RemoveMethod ?? member.AddMethod).IsPublic;
            _isStatic = (member.AddMethod ?? member.RemoveMethod).IsStatic;
            _isVirtual = (member.AddMethod ?? member.RemoveMethod).IsVirtual;
        }

        public override bool IsPublic
        {
            get { return _isPublic; }
        }

        public override bool IsStatic
        {
            get { return _isStatic; }
        }

        public bool IsVirtual
        {
            get { return _isVirtual; }
        }

        public bool IsAbstract
        {
            get { return _isAbstract; }
        }

        public ITypeReference EventHandlerType
        {
            get { return _eventHandlerType; }
        }
    }
}