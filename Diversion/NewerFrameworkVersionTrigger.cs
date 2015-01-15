namespace Diversion
{
    public class NewerFrameworkVersionTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return change.Old.FrameworkVersion < change.New.FrameworkVersion;
        }
    }
}
