namespace Garden
{
    public struct TreeTypeConfig
    {
        public TreeType TreeType;
        public ParamType ParamType;
        public int ChanceInPercent;

        public TreeTypeConfig(TreeType treeType, ParamType paramType, int chanceInPercent)
        {
            TreeType = treeType;
            ParamType = paramType;
            ChanceInPercent = chanceInPercent;
        }
    }
}