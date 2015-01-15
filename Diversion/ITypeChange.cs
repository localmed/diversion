namespace Diversion
{
    public interface ITypeChange : IChange<ITypeInfo>
    {
        IChanges<IMemberInfo, IMemberChange> MemberChanges { get; }
    }
}