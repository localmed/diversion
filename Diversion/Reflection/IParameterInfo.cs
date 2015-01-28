namespace Diversion.Reflection
{
    public interface IParameterInfo : IAttributable
    {
        ITypeInfo Type { get; }
        string Name { get; }
    }
}