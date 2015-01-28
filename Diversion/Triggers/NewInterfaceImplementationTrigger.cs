using System.Linq;

namespace Diversion.Triggers
{
    /// <summary>
    /// A version trigger that triggers when an interface implementation is added to a type.
    /// </summary>
    public class NewInterfaceImplementationTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return change.TypeChanges.Changes.Any(tc => tc.InterfaceChanges.Added.Any());
        }
    }
}