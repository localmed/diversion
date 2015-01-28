using System.Reflection;

namespace Diversion.Reflection
{
    internal class NvEventInfo : NvMemberInfo, IEventInfo
    {
        private readonly EventInfo _member;

        public NvEventInfo(EventInfo member)
            : base(member)
        {
            _member = member;
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
            get { return new NvTypeInfo(_member.EventHandlerType); }
        }
    }
}