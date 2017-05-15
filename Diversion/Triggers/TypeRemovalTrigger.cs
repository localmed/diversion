using System.Linq;

namespace Diversion.Triggers
{
    /// <summary>
    /// A version trigger that triggers when a public or protected type is removed from an assembly.
    /// </summary>
    public class TypeRemovalTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyDiversion diversion)
        {
            return diversion.TypeDiversions.Removed.Any(t => t.IsOnApiSurface) || diversion.TypeDiversions.Diverged.Any(t => t.Old.IsOnApiSurface && !t.New.IsOnApiSurface);
        }
    }
}