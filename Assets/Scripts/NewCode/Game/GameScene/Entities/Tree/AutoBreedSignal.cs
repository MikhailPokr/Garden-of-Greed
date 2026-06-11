namespace Garden
{
    public struct AutoBreedSignal : ISignal
    {
        public TreeData TreeData;
        public AutoBreedSignal(TreeData treeData)
        {
            TreeData = treeData;
        }
    }
}