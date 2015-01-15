namespace Diversion
{
    public class NewMemberOnInterfaceTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return false;
            //return change.TypeChanges.Changes.Where(tc => tc.New.IsInterface? && tc.MemberChanges.Added.Any());
        }
    }
}