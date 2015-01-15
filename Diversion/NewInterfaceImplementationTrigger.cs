namespace Diversion
{
    public class NewInterfaceImplementationTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return false;
        }
    }
}