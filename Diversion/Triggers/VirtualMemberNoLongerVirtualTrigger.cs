using System.Linq;
using Diversion.Reflection;

namespace Diversion.Triggers
{
    /// <summary>
    /// A version trigger that triggers when a virtual member is changed to nonvirtual.
    /// </summary>
    public class VirtualMemberNoLongerVirtualTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyDiversion diversion)
        {
            return diversion.TypeDiversions.Diverged.AsParallel().Any(tc => tc.MemberDiversions.Diverged.AsParallel().OfType<IDiversion<IVirtualizable>>().Any(mc => !mc.New.IsVirtual && mc.Old.IsVirtual));
        }
    }

}