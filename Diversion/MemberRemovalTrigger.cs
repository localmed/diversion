using System.Linq;

namespace Diversion
{
    /// <summary>
    /// A version trigger that triggers when a public or protected member has been removed from a public or protected type.
    /// </summary>
    public class MemberRemovalTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return change.TypeChanges.Changes.Any(tc => tc.MemberChanges.Removed.Any());
        }
    }
}