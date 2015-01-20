using System.Linq;

namespace Diversion
{
    /// <summary>
    /// A version trigger that triggers when a new type is added to an assembly.
    /// </summary>
    public class NewTypeTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return change.TypeChanges.Added.Any();
        }
    }
}