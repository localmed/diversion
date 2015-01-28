namespace Diversion.Reflection
{
    public interface IEventInfo : IMemberInfo, IVirtualizable
    {
        ITypeInfo EventHandlerType { get; }
    }
}