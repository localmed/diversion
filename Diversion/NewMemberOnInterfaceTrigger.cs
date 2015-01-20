using System.Linq;

namespace Diversion
{
    /// <summary>
    /// A version trigger that triggers when a new member is added to an interface.
    /// </summary>
    public class NewMemberOnInterfaceTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return change.TypeChanges.Changes.Any(tc => tc.New.IsInterface && tc.MemberChanges.Added.Any());
        }
    }
}