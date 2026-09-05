namespace Garden
{
    public interface IClickSignal : ISignal
    {
        InteractionType InteractionType { get; }
    }
}