namespace Diversion.Reflection
{
    public interface IParameterInfo : IAttributable, IIdentifiable
    {
        ITypeInfo Type { get; }
        string Name { get; }
    }
}