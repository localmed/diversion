using System.Linq;

namespace Diversion.Triggers
{
    /// <summary>
    /// A version trigger that triggers when a public or protected type is removed from an assembly.
    /// </summary>
    public class TypeRemovalTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return change.TypeChanges.Removed.Any();
        }
    }
}