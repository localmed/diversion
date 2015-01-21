using System.Linq;

namespace Diversion
{
    /// <summary>
    /// A version trigger that is triggered when an abstract member is added to a type.
    /// </summary>
    public class NewAbstractMemberTrigger : IVersionTrigger
    {
        public bool IsTriggered(IAssemblyChange change)
        {
            return change.TypeChanges.Changes.Any(tc => tc.MemberChanges.Added.OfType<IVirtualizable>().Any(mi => mi.IsAbstract));
        }
    }
}