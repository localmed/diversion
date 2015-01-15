using System.Linq;

namespace Diversion
{
    public class NewMemberTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return change.TypeChanges.Changes.Any(tc => tc.MemberChanges.Added.Any());
        }
    }
}