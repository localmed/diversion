namespace Diversion.Reflection
{
    public interface IEventInfo : IMemberInfo, IVirtualizable
    {
        ITypeReference EventHandlerType { get; }
    }
}