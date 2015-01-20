using System.Linq;

namespace Diversion
{
    /// <summary>
    /// A version trigger that is triggered when the implementation of an interface is removed from a public or protected type.
    /// </summary>
    public class InterfaceImplementationRemovalTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return change.TypeChanges.Changes.Any(t => t.InterfaceChanges.Removed.Any());
        }
    }
}