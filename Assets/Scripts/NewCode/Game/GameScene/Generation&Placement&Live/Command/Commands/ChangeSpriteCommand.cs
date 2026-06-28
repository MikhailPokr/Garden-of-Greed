namespace Garden
{
    public class ChangeSpriteCommand : ICommand
    {
        public int Value;

        public ChangeSpriteCommand(int value)
        {
            Value = value;
        }
        public ChangeSpriteCommand(TreeSpriteCommandsLegend value)
        {
            Value = (int)value;
        }
    }
}