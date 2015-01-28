using System;
using System.Collections.Generic;

namespace Diversion.Reflection
{
    public interface IAssemblyInfo: IAttributable
    {
        string Name { get; }
        Version Version { get; }
        Version FrameworkVersion { get; }
        byte[] MD5 { get; }
        IEnumerable<ITypeInfo> Types { get; }
    }
}