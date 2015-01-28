namespace Diversion
{
    public interface IChanges<out T> : IChanges<T, IChange<T>> { }

    public interface IChanges<out T, out TC> : ICollectionChanges<T>, IItemChanges<T, TC> where TC : IChange<T>
    {
    }
}