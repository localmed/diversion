using System.Linq;

namespace Diversion
{
    public interface IDiversions<out T> : IDiversions<T, IDiversion<T>> { }

    public interface IDiversions<out T, out TC> : ICollectionDiversions<T>, IItemDiversions<T, TC> where TC : IDiversion<T>
    {
    }

    public static class DiversionsExtensions
    {
        public static bool HasDiverged<T, TC>(this IDiversions<T, TC> it) where TC : IDiversion<T>
        {
            return it.Added.Any() || it.Removed.Any() || it.Diverged.Any();
        }
        public static bool HasDiverged<T>(this ICollectionDiversions<T> it)
        {
            return it.Added.Any() || it.Removed.Any();
        }
        public static bool HasDiverged<T, TC>(this IItemDiversions<T, TC> it) where TC : IDiversion<T>
        {
            return it.Diverged.Any();
        }
    }
}