namespace Diversion
{
    public interface ITypeChange : IChange<ITypeInfo>
    {
        IChanges<IMemberInfo> MemberChanges { get; }
        ICollectionChanges<ITypeInfo> InterfaceChanges { get; }
        IChanges<IAttributeInfo> AttributeChanges { get; } 
    }

    public interface IAttributeInfo
    {
    }
}