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
    }
}