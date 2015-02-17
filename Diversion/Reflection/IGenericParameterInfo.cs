using System.Collections.Generic;

namespace Diversion.Reflection
{
    public interface IGenericParameterInfo : ITypeReference
    {
        bool RequiresDefaultConstructor { get; }
        GenericTypeRequirement TypeRequirement { get; }
        GenericTypeVariance TypeVariance { get; }
        ITypeReference Base { get; }
        IReadOnlyList<ITypeReference> Interfaces { get; } 
    }
}