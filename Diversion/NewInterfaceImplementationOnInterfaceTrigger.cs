namespace Diversion
{
    public class NewInterfaceImplementationOnInterfaceTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return false;
        }
    }
}