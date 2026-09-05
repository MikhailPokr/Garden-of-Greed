namespace Garden
{
    public class FruitProduceCommand : ICommand
    {
        private readonly TreeData _treeData;

        public FruitProduceCommand(TreeData treeData)
        {
            _treeData = treeData;
        }

        public void Use()
        {
            SignalBus<FruitProduceSignal>.Fire(new FruitProduceSignal(_treeData));
        }
    }
}