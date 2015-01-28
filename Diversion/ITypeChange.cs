using Diversion.Reflection;

namespace Diversion
{
    public interface ITypeChange : IChange<ITypeInfo>
    {
        IChanges<IMemberInfo> MemberChanges { get; }
        ICollectionChanges<ITypeInfo> InterfaceChanges { get; }
        //ICollectionChanges<IAttributeInfo> AttributeChanges { get; }
    }
}