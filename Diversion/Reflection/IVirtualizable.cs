namespace Diversion.Reflection
{
    public interface IVirtualizable
    {
        bool IsVirtual { get; }
        bool IsAbstract { get; }
    }
}