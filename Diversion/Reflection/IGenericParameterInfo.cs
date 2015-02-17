namespace Diversion.Reflection
{
    public interface IGenericParameterInfo : ITypeReference
    {
        bool RequiresDefaultConstructor { get; }
        GenericTypeRequirement TypeRequirement { get; }
        GenericTypeVariance TypeVariance { get; }
    }
}