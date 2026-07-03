namespace Garden
{
    internal class ChangeHpSignal : ISignal
    {
        public readonly int Delta;

        public ChangeHpSignal(int delta)
        {
            Delta = delta;
        }
    }
}