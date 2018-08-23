using NuGet.Frameworks;
using NuGet.Versioning;
using System;
using System.Collections.Generic;

namespace Diversion.Reflection
{
    public interface IAssemblyInfo : IAttributable, IIdentifiable, IFrameworkSpecific, INuGetVersionable
    {
        Guid Mvid { get; }
        string Name { get; }
        IReadOnlyList<ITypeInfo> Types { get; }
    }
}