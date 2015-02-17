namespace Diversion
{
    public interface IDiversion<out T>
    {
        bool HasDiverged();
        T Old { get; }
        T New { get; }
    }
}