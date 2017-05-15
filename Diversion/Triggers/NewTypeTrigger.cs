using System.Linq;

namespace Diversion.Triggers
{
    /// <summary>
    /// A version trigger that triggers when a new type is added to an assembly.
    /// </summary>
    public class NewTypeTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyDiversion diversion)
        {
            return diversion.TypeDiversions.Added.Any(t => t.IsOnApiSurface) || diversion.TypeDiversions.Diverged.Any(t => !t.Old.IsOnApiSurface && t.New.IsOnApiSurface);
        }
    }
}