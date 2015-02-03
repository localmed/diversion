using System.Collections.Generic;

namespace Diversion.Reflection
{
    public interface IConstructorInfo : IMemberInfo
    {
        IReadOnlyList<IParameterInfo> Parameters { get; }
    }
}