namespace Garden
{
    public interface ITreePalette
    {
        int GetStageSpritesCount();
        int GetSpritesCount(TreeType treeType);
        int GetWoodColorsCount(TreeType treeType);
    }
}