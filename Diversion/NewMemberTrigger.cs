using System.Linq;

namespace Diversion
{
    /// <summary>
    /// A version trigger that triggers when a member is added to a type.
    /// </summary>
    public class NewMemberTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return change.TypeChanges.Changes.Any(tc => tc.MemberChanges.Added.Any());
        }
    }
}