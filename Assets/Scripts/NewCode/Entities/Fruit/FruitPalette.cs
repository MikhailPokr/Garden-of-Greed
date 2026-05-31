using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "FruitPalette", menuName = "Game/Palette/FruitPalette")]
    public class FruitPalette : ScriptableObject
    {
        [field: SerializeField] public List<Sprite> FruitSprites { get; private set; }
        [field: SerializeField] public List<Sprite> FruitEvilSprites { get; private set; }
        [field: SerializeField] public List<Color> FruitColors { get; private set; }
        [field: SerializeField] public List<Color> FruitEvilColors { get; private set; }
    }
}