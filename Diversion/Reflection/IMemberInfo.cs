namespace Diversion.Reflection
{
    public interface IMemberInfo : IAttributable, IIdentifiable
    {
        ITypeReference DeclaringType { get; }
        string Name { get; }
        bool IsPublic { get; }
        bool IsStatic { get; }
        byte[] Implementation { get; }
    }
}