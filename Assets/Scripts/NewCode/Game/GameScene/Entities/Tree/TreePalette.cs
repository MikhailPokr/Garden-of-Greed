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
        [field: SerializeField] public List<Color> WoodColors { get; private set; }
        public int StageSpritesCount => StageSprites.Count;
        public int TreeEvilSpritesCount => TreeSprites.Count;
        public int TreeSpritesCount => TreeSprites.Count;
        public int WoodColorsCount => WoodColors.Count;
    }
}