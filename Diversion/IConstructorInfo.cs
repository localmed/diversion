using System.Collections.Generic;

namespace Diversion
{
    public interface IConstructorInfo : IMemberInfo
    {
        IEnumerable<IParameterInfo> Parameters { get; }
    }
}