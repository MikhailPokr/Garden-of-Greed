using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "TreePalette", menuName = "Garden/Palette/TreePalette")]
    public class TreePalette : BasePalette, ITreePalette
    {
        [field: SerializeField] public List<Sprite> StageSprites { get; private set; }
        [field: SerializeField] public List<Sprite> TreeSprites { get; private set; }
        [field: SerializeField] public List<Sprite> CrownSprites { get; set; }
        [field: SerializeField] public List<Sprite> TreeEvilSprites { get; private set; }
        [field: SerializeField] public List<Sprite> EvilCrownSprites { get; set; }
        [field: SerializeField] public List<Color> WoodColors { get; private set; }
        [field: SerializeField] public List<Color> EvilWoodColors { get; private set; }
        public int GetStageSpritesCount() => StageSprites.Count;

        public int GetSpritesCount(TreeType treeType) => treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => TreeEvilSprites.Count,
            _ => TreeSprites.Count
        };

        public int GetWoodColorsCount(TreeType treeType) => treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => EvilWoodColors.Count,
            _ => WoodColors.Count
        };
    }
}