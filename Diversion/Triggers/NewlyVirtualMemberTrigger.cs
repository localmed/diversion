using System.Linq;
using Diversion.Reflection;

namespace Diversion.Triggers
{
    /// <summary>
    /// A version trigger that triggers when a nonvirtual member is made virtual.
    /// </summary>
    public class NewlyVirtualMemberTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return change.TypeChanges.Changes.Any(tc => tc.MemberChanges.Changes.OfType<IChange<IVirtualizable>>().Any(mc => !mc.Old.IsVirtual && mc.New.IsVirtual));
        }
    }
}