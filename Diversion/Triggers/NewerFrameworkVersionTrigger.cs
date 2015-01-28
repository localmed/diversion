namespace Diversion.Triggers
{
    /// <summary>
    /// A version trigger that triggers when the target .net framework of the newly built assembly is a more recent version than the old assembly.
    /// </summary>
    public class NewerFrameworkVersionTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return change.Old.FrameworkVersion < change.New.FrameworkVersion;
        }
    }
}
