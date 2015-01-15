using System;
using System.Collections.Generic;

namespace Diversion
{
    public interface IMemberInfo
    {
        string Name { get; }
        Type Type { get; }
        IEnumerable<IParameterInfo> Parameters { get; }
        bool IsVirtual { get; }
    }
}