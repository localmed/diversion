using Diversion.Reflection;

namespace Diversion
{
    public interface ITypeDiversion : IDiversion<ITypeInfo>
    {
        IDiversions<IMemberInfo> MemberDiversions { get; }
        ICollectionDiversions<ITypeReference> InterfaceDiversions { get; }
        ICollectionDiversions<IAttributeInfo> AttributeDiversions { get; }
    }
}