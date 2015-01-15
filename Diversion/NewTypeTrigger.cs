using System.Linq;

namespace Diversion
{
    public class NewTypeTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return change.TypeChanges.Added.Any();
        }
    }
}