using System;
using System.Collections.Generic;

namespace Diversion
{
    public interface IMemberInfo
    {
        string Name { get; }
        Type Type { get; }
        IEnumerable<IParameterInfo> Parameters { get; }
        bool IsNonVirtual { get; }
        bool IsVirtual { get; }
        bool IsOverride { get; }
        bool IsAbstract { get; }
        bool IsSealed { get; }
    }
}