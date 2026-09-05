using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "FruitPalette", menuName = "Garden/Palette/FruitPalette")]
    public class FruitPalette : BasePalette, IFruitPalette
    {
        [field: SerializeField] public List<Sprite> FruitSprites { get; private set; }
        [field: SerializeField] public List<Sprite> FruitEvilSprites { get; private set; }
        [field: SerializeField] public List<Color> FruitColors { get; private set; }
        [field: SerializeField] public List<Color> FruitEvilColors { get; private set; }
        public int SpriteCount => FruitSprites.Count;
        public int ColorCount => FruitColors.Count;
        public int EvilSpriteCount => FruitEvilSprites.Count;
        public int EvilColorCount => FruitEvilColors.Count;
        public Sprite GetSprite(TreeType treeType, int index) => treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => FruitEvilSprites[index],
            _ => FruitSprites[index]
        };
        public Color GetColor(TreeType treeType, int index) => treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => FruitEvilColors[index],
            _ => FruitColors[index]
        };
        public int GetSpritesCount(TreeType treeType) => treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => FruitEvilSprites.Count,
            _ => FruitSprites.Count
        };
        public int GetColorsCount(TreeType treeType) => treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => FruitEvilColors.Count,
            _ => FruitColors.Count
        };
    }
}