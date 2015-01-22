namespace Diversion
{
    public interface IEventInfo : IMemberInfo, IVirtualizable
    {
        ITypeInfo EventHandlerType { get; }
    }
}