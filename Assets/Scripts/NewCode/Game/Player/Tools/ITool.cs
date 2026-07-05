namespace Garden
{
    public interface ITool
    {
        ToolType Type { get; }
        void Activate();
        void Process(InteractionData signal);
    }
}