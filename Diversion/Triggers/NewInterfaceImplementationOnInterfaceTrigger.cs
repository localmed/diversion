using System.Linq;

namespace Diversion.Triggers
{
    /// <summary>
    /// A version trigger that triggers when a base interface is added to an existing interface.
    /// </summary>
    public class NewInterfaceImplementationOnInterfaceTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return change.TypeChanges.Changes.Any(tc => tc.New.IsInterface && tc.InterfaceChanges.Added.Any());
        }
    }
}