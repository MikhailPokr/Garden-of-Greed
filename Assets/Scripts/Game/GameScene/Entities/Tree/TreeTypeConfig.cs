namespace Garden
{
    public struct TreeTypeConfig
    {
        public TreeType TreeType;
        public ParamType ParamType;
        public float Chance;

        public TreeTypeConfig(TreeType treeType, ParamType paramType, float chance)
        {
            TreeType = treeType;
            ParamType = paramType;
            Chance = chance;
        }
    }
}