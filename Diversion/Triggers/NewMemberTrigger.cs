using System.Linq;

namespace Diversion.Triggers
{
    /// <summary>
    /// A version trigger that triggers when a member is added to a type.
    /// </summary>
    public class NewMemberTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyDiversion diversion)
        {
            return diversion.TypeDiversions.Diverged.AsParallel().Any(tc => tc.New.IsOnApiSurface && (tc.MemberDiversions.Added.Any(m => m.IsOnApiSurface) || tc.MemberDiversions.Diverged.Any(m => !m.Old.IsOnApiSurface && m.New.IsOnApiSurface)));
        }
    }
}