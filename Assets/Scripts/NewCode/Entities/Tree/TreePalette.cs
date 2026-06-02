using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "TreePalette", menuName = "Garden/Palette/TreePalette")]
    public class TreePalette : BasePalette
    {
        [field: SerializeField] public List<Sprite> StageSprites { get; private set; }
        [field: SerializeField] public List<Sprite> TreeSprites { get; private set; }
        [field: SerializeField] public List<Sprite> CrownSprites { get; set; }
        [field: SerializeField] public List<Sprite> TreeEvilSprites { get; private set; }
        [field: SerializeField] public List<Color> WoodColors { get; private set; }
    }
}