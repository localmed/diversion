using System.Linq;

namespace Diversion
{
    /// <summary>
    /// A version trigger that triggers when a virtual member is changed to nonvirtual.
    /// </summary>
    public class VirtualMemberNoLongerVirtualTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return change.TypeChanges.Changes.Any(tc => tc.MemberChanges.Changes.Any(mc => mc.New.IsNonVirtual && mc.Old.IsVirtual));
        }
    }
}