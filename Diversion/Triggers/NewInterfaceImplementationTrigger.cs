using System.Linq;

namespace Diversion.Triggers
{
    /// <summary>
    /// A version trigger that triggers when an interface implementation is added to a type.
    /// </summary>
    public class NewInterfaceImplementationTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyDiversion diversion)
        {
            return diversion.TypeDiversions.Diverged.AsParallel().Any(tc => tc.InterfaceDiversions.Added.Any());
        }
    }
}