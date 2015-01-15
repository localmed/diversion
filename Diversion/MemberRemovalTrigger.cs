using System.Linq;

namespace Diversion
{
    public class MemberRemovalTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return change.TypeChanges.Changes.Any(tc => tc.MemberChanges.Removed.Any());
        }
    }
}