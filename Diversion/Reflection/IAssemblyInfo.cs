using System;
using System.Collections.Generic;

namespace Diversion.Reflection
{
    public interface IAssemblyInfo : IAttributable, IIdentifiable
    {
        string Name { get; }
        Version Version { get; }
        Version FrameworkVersion { get; }
        IReadOnlyList<ITypeInfo> Types { get; }
    }
}