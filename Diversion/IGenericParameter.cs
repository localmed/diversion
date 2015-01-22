using System.Collections.Generic;

namespace Diversion
{
    public interface IGenericParameter
    {
        string Name { get; }
        IEnumerable<ITypeInfo> Constraints { get; }
        bool RequiresDefaultConstructor { get; }
        GenericTypeRequirement TypeRequirement { get; }
    }
}