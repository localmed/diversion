namespace Diversion
{
    public interface IVersionTrigger
    {
        bool IsTriggered(IAssemblyChange change);
    }
}
