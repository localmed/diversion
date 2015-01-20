namespace Diversion
{
    /// <summary>
    /// An interface for triggering a version number change based on the changes between two builds of an assembly.
    /// </summary>
    public interface IVersionTrigger
    {
        /// <summary>
        /// Returns whether or not the trigger condition has been met based on the changes represented in <paramref name="change"/>.
        /// </summary>
        /// <param name="change">Represents the changes between an older and newer version of an assembly.</param>
        /// <returns>Returns <c>true</c> if the trigger conditions have been met; <c>false</c> otherwise.</returns>
        bool IsTriggered(IAssemblyChange change);
    }
}
