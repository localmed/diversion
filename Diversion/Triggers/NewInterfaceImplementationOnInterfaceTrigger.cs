using System.Linq;

namespace Diversion.Triggers
{
    /// <summary>
    /// A version trigger that triggers when a base interface is added to an existing interface.
    /// </summary>
    public class NewInterfaceImplementationOnInterfaceTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyDiversion diversion)
        {
            return diversion.TypeDiversions.Diverged.AsParallel().Any(tc => tc.New.IsInterface && tc.InterfaceDiversions.Added.Any());
        }
    }
}