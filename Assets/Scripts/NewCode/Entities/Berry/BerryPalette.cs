using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "BerryPalette", menuName = "Game/Palette/BerryPalette")]
    public class BerryPalette : ScriptableObject
    {
        [field: SerializeField] public List<Sprite> BerrySprites { get; private set; }
        [field: SerializeField] public List<Sprite> BerryEvilSprites { get; private set; }
        [field: SerializeField] public List<Color> BerryColors { get; private set; }
        [field: SerializeField] public List<Color> BerryEvilColors { get; private set; }
    }
}