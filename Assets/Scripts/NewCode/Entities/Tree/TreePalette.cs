using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "TreePalette", menuName = "Game/Palette/TreePalette")]
    public class TreePalette : ScriptableObject, IPalette
    {
        [field: SerializeField] public List<Sprite> StageSprites { get; private set; }
        [field: SerializeField] public List<Sprite> TreeSprites { get; private set; }
        [field: SerializeField] public List<Sprite> TreeEvilSprites { get; private set; }
        [field: SerializeField] public List<Color> WoodColors { get; private set; }
    }
}