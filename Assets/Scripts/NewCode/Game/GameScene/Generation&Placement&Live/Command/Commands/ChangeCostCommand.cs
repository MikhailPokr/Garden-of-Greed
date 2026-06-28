namespace Garden
{
    public class ChangeCostCommand : ICommand
    {
        public int Value;
        public ChangeCostCommand(int value)
        {
            Value = value;
        }
    }
}