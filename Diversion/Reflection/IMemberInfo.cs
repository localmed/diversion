namespace Diversion.Reflection
{
    public interface IMemberInfo : IAttributable, IIdentifiable
    {
        ITypeInfo DeclaringType { get; }
        string Name { get; }
        bool IsPublic { get; }
        bool IsStatic { get; }
    }
}