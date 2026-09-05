namespace Garden
{
    public struct ColorModeChangedSignal : ISignal
    {
        public bool IsColored;
        
        public ColorModeChangedSignal(bool isColored)
        {
            IsColored = isColored;
        }
    }
}