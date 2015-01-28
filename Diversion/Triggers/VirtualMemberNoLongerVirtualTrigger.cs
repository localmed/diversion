using System.Linq;
using Diversion.Reflection;

namespace Diversion.Triggers
{
    /// <summary>
    /// A version trigger that triggers when a virtual member is changed to nonvirtual.
    /// </summary>
    public class VirtualMemberNoLongerVirtualTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return change.TypeChanges.Changes.Any(tc => tc.MemberChanges.Changes.OfType<IChange<IVirtualizable>>().Any(mc => !mc.New.IsVirtual && mc.Old.IsVirtual));
        }
    }

}