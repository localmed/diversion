using System;
using System.Collections.Generic;

namespace Diversion
{
    public interface IAssemblyInfo
    {
        string Name { get; }
        Version Version { get; }
        Version FrameworkVersion { get; }
        byte[] MD5 { get; }
        IEnumerable<ITypeInfo> Types { get; }
    }
}