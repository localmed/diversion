namespace Diversion.Reflection
{
    public interface IParameterInfo : IAttributable, IIdentifiable
    {
        ITypeReference Type { get; }
        string Name { get; }
    }
}