namespace Garden
{
    internal struct NumPressedSignal : ISignal
    {
        public int Num;
        public NumPressedSignal(int num)
        {
            Num = num;
        }
    }
}