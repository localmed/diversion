namespace Diversion
{
    public class NewAbstractMemberTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return false;
        }
    }
}