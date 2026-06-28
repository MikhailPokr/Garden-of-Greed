namespace Garden
{
    public class BreedCommand : ICommand
    {
        private readonly TreeData _treeData;

        public BreedCommand(TreeData treeData)
        {
            _treeData = treeData;
        }

        public void Use()
        {
            SignalBus<FruitProduceSignal>.Fire(new FruitProduceSignal(_treeData));
        }
    }
}