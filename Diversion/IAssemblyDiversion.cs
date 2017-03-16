using Diversion.Reflection;

namespace Diversion
{
    public interface IAssemblyDiversion : IDiversion<IAssemblyInfo>
    {
        IDiversions<ITypeInfo, ITypeDiversion> TypeDiversions { get; }
        ICollectionDiversions<IAttributeInfo> AttributeDiversions { get; }
    }
}