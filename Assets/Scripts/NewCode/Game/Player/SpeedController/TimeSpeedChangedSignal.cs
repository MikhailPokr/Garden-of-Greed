namespace Garden
{
    internal struct TimeSpeedChangedSignal : ISignal
    {
        public int Value;
        public TimeSpeedChangedSignal(int value)
        {
            Value = value;
        }
    }
}