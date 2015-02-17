namespace Diversion.Reflection
{
    public interface ITypeReference : IIdentifiable
    {
        ITypeReference DeclaringType { get; }
        string Name { get; }
        string Namespace { get; }
    }
}