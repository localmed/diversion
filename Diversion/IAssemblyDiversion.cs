using Diversion.Reflection;

namespace Diversion
{
    public interface IAssemblyDiversion : IDiversion<IAssemblyInfo>
    {
        IDiversions<ITypeInfo, ITypeDiversion> TypeDiversions { get; }
        //ICollectionChanges<IAttributeInfo> AttributeChanges { get; } 
    }
}