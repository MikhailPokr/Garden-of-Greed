namespace Garden
{
    public class CounterUpCommand : ICommand
    {
        public readonly CellData CellData;

        public CounterUpCommand()
        {
        }

        public CounterUpCommand(CellData cellData)
        {
            CellData = cellData;
        }
    }
}