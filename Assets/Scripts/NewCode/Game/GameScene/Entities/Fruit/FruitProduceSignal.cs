namespace Garden
{
    public class FruitProduceSignal : ISignal
    {
        public TreeData TreeData;
        public FruitProduceSignal(TreeData treeData)
        {
            TreeData = treeData;
        }
    }
}