using System.Diagnostics.Contracts;
using System.Reflection;

namespace Diversion.Reflection
{
    class NvEventInfo : NvMemberInfo, IEventInfo
    {
        private readonly EventInfo _member;
        private readonly ITypeInfo _eventHandlerType;

        public NvEventInfo(IReflectionInfoFactory reflectionInfoFactory, EventInfo member)
            : base(reflectionInfoFactory, member)
        {
            Contract.Requires(reflectionInfoFactory != null);
            Contract.Requires(member != null);
            _member = member;
            _eventHandlerType = reflectionInfoFactory.FromReflection(_member.EventHandlerType);
        }

        public override bool IsPublic
        {
            get { return (_member.AddMethod ?? _member.RemoveMethod).IsPublic || (_member.RemoveMethod ?? _member.AddMethod).IsPublic; }
        }

        public override bool IsStatic
        {
            get { return (_member.AddMethod ?? _member.RemoveMethod).IsStatic; }
        }

        public bool IsVirtual
        {
            get { return (_member.AddMethod ?? _member.RemoveMethod).IsVirtual; }
        }

        public bool IsAbstract
        {
            get { return (_member.AddMethod ?? _member.RemoveMethod).IsAbstract; }
        }

        public ITypeInfo EventHandlerType
        {
            get { return _eventHandlerType; }
        }
    }
}