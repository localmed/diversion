using System.Linq;
using Diversion.Reflection;

namespace Diversion.Triggers
{
    /// <summary>
    /// A version trigger that triggers when a nonvirtual member is made virtual.
    /// </summary>
    public class NewlyVirtualMemberTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyDiversion diversion)
        {
            return diversion.TypeDiversions.Diverged.AsParallel().Any(tc => tc.New.IsOnApiSurface && tc.MemberDiversions.Diverged.AsParallel().OfType<IDiversion<IVirtualizable>>().Any(mc => !mc.Old.IsVirtual && mc.New.IsVirtual));
        }
    }
}