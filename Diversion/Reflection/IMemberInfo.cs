namespace Diversion.Reflection
{
    public interface IMemberInfo : IAttributable, IIdentifiable
    {
        ITypeReference BaseDeclaringType { get; }
        ITypeReference DeclaringType { get; }
        string Name { get; }
        bool IsOnApiSurface { get; }
        bool IsPublic { get; }
        bool IsStatic { get; }
    }
}