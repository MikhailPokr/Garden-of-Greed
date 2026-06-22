namespace Garden
{
    public interface ITool
    {
        ToolType Type { get; }
        void Activate();
        void Process(IClickSignal signal);
    }
}