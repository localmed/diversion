using System.Linq;

namespace Diversion
{
    /// <summary>
    /// A version trigger that triggers when a nonvirtual member is made virtual.
    /// </summary>
    public class NewlyVirtualMemberTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return change.TypeChanges.Changes.Any(tc => tc.MemberChanges.Changes.Any(mc => mc.Old.IsNonVirtual && mc.New.IsVirtual));
        }
    }
}