namespace Diversion
{
    public interface IChange<out T>
    {

        T Old { get; }
        T New { get; }
    }
}