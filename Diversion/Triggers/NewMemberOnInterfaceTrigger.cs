using System.Linq;

namespace Diversion.Triggers
{
    /// <summary>
    /// A version trigger that triggers when a new member is added to an interface.
    /// </summary>
    public class NewMemberOnInterfaceTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyDiversion diversion)
        {
            return diversion.TypeDiversions.Diverged.AsParallel().Any(tc => tc.New.IsInterface && tc.MemberDiversions.Added.Any());
        }
    }
}