using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "GrassPalette", menuName = "Game/Palette/GrassPalette")]
    public class GrassPalette : ScriptableObject
    {
        [field: SerializeField] public List<Sprite> GrassSprites { get; private set; }
    }
}