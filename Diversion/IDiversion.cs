namespace Diversion
{
    public interface IDiversion<out T> : IIdentifiable
    {
        bool HasDiverged();
        T Old { get; }
        T New { get; }
    }
}