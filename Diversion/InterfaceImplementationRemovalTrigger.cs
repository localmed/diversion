namespace Diversion
{
    public class InterfaceImplementationRemovalTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return false;
        }
    }
}