namespace Diversion
{
    public class VirtualMemberNoLongerVirtualTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return false;
        }
    }
}