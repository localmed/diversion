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
            return diversion.TypeDiversions.Diverged.AsParallel().Any(tc => tc.New.IsOnApiSurface && (tc.MemberDiversions.Removed.Any(m => m.IsOnApiSurface) || tc.MemberDiversions.Diverged.Any(m => m.Old.IsOnApiSurface && !m.New.IsOnApiSurface)));
        }
    }
}