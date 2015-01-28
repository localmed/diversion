using System.Collections.Generic;

namespace Diversion.Reflection
{
    public interface IConstructorInfo : IMemberInfo
    {
        IEnumerable<IParameterInfo> Parameters { get; }
    }
}