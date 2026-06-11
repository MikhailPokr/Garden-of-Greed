using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "GrassPalette", menuName = "Garden/Palette/GrassPalette")]
    public class GrassPalette : BasePalette
    {
        [field: SerializeField] public List<Sprite> GrassSprites { get; private set; }
    }
}