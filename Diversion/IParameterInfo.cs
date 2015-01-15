using System;

namespace Diversion
{
    public interface IParameterInfo
    {
        Type Type { get; }
        string Name { get; }
    }
}