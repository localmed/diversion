using System;

namespace Diversion
{
    public interface IParameterInfo : IAttributable
    {
        ITypeInfo Type { get; }
        string Name { get; }
    }
}