namespace Garden
{
    public interface ITool
    {
        ToolType Type { get; }
        bool Locked { get; }
        void Activate();
        void Lock(bool locked);
        void Process(InteractionData signal);
    }
}