namespace Garden
{
    internal struct ChangeHpSignal : ISignal
    {
        public readonly int Delta;

        public ChangeHpSignal(int delta)
        {
            Delta = delta;
        }
    }
}