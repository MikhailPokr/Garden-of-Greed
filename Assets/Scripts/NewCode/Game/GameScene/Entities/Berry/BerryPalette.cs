using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "BerryPalette", menuName = "Garden/Palette/BerryPalette")]
    public class BerryPalette : BasePalette
    {
        [field: SerializeField] public List<Sprite> BerrySprites { get; private set; }
        [field: SerializeField] public List<Sprite> BerryEvilSprites { get; private set; }
        [field: SerializeField] public List<Color> BerryColors { get; private set; }
        [field: SerializeField] public List<Color> BerryEvilColors { get; private set; }
    }
}