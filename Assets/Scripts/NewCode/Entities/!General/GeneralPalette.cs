using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "GeneralPalette", menuName = "Game/Palette/GeneralPalette")]
    public class GeneralPalette : ScriptableObject
    {
        [field: SerializeField] public Color NoColor { get; private set; }
        [field: SerializeField] public Color NormalColor { get; private set; }
        [field: SerializeField] public Color EvilColor { get; private set; }
    }
}