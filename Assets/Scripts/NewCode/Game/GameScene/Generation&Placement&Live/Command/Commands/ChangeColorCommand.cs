namespace Garden
{
    public class ChangeColorCommand : ICommand
    {
        public int Value;

        public ChangeColorCommand(int value)
        {
            Value = value;
        }
        public ChangeColorCommand(TreeColorCommandsLegend value)
        {
            Value = (int)value;
        }
    }
}