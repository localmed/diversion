using System.Linq;

namespace Diversion.Triggers
{
    /// <summary>
    /// A version trigger that is triggered when the implementation of an interface is removed from a public or protected type.
    /// </summary>
    public class InterfaceImplementationRemovalTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyDiversion diversion)
        {
            return diversion.TypeDiversions.Diverged.AsParallel().Any(t => t.New.IsOnApiSurface && t.InterfaceDiversions.Removed.Any());
        }
    }
}