using System.Linq;

namespace Diversion.Triggers
{
    /// <summary>
    /// A version trigger that triggers when a public or protected member has been removed from a public or protected type.
    /// </summary>
    public class MemberRemovalTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyDiversion diversion)
        {
            return diversion.TypeDiversions.Diverged.AsParallel().Any(tc => tc.MemberDiversions.Removed.Any());
        }
    }
}