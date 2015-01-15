namespace Diversion
{
    public interface IAssemblyChange : IChange<IAssemblyInfo>
    {
        IChanges<ITypeInfo, ITypeChange> TypeChanges { get; }
    }
}