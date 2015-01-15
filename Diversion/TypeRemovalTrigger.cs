using System.Linq;

namespace Diversion
{
    public class TypeRemovalTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return change.TypeChanges.Removed.Any();
        }
    }
}