using System.Collections.Generic;

namespace Diversion.Reflection
{
    public interface IGenericParameterInfo
    {
        string Name { get; }
        IEnumerable<ITypeInfo> Constraints { get; }
        bool RequiresDefaultConstructor { get; }
        GenericTypeRequirement TypeRequirement { get; }
    }
}