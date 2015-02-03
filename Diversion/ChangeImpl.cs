namespace Diversion
{
    internal class ChangeImpl<T> : IChange<T>
    {
        public ChangeImpl(T old, T @new)
        {
            Old = old;
            New = @new;
        }

        public T Old { get; private set; }

        public T New { get; private set; }
    }
}