using System.Linq;
using Diversion.Reflection;

namespace Diversion.Triggers
{
    /// <summary>
    /// A version trigger that is triggered when an abstract member is added to a type.
    /// </summary>
    public class NewAbstractMemberTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyDiversion diversion)
        {
            return diversion.TypeDiversions.Diverged.AsParallel().Any(tc => tc.New.IsOnApiSurface && tc.MemberDiversions.Added.AsParallel().OfType<IVirtualizable>().Any(mi => mi.IsAbstract));
        }
    }
}