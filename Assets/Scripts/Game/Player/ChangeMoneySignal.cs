namespace Garden
{
    public struct ChangeMoneySignal : ISignal
    {
        public int Delta { get; set; }
        public ChangeMoneySignal(int delta)
        {
            Delta = delta;
        }

    }
}