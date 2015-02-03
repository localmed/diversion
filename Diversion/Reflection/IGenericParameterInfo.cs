namespace Diversion.Reflection
{
    public interface IGenericParameterInfo : ITypeInfo
    {
        bool RequiresDefaultConstructor { get; }
        GenericTypeRequirement TypeRequirement { get; }
        GenericTypeVariance TypeVariance { get; }
    }
}