using System;
using System.Collections.Generic;

namespace Diversion.Reflection
{
    public interface IAssemblyInfo : IAttributable
    {
        string Name { get; }
        Version Version { get; }
        Version FrameworkVersion { get; }
        byte[] Hash { get; }
        IReadOnlyList<ITypeInfo> Types { get; }
    }
}