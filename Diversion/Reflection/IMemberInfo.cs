namespace Diversion.Reflection
{
    public interface IMemberInfo : IAttributable
    {
        string Name { get; }
        bool IsPublic { get; }
        bool IsStatic { get; }
    }
}