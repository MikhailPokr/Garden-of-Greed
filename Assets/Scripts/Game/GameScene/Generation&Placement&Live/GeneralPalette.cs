using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "GeneralPalette", menuName = "Garden/Palette/GeneralPalette")]
    public class GeneralPalette : ScriptableObject
    {
        [field: SerializeField] public Color NoColor { get; private set; }
        [field: SerializeField] public Color NormalColor { get; private set; }
        [field: SerializeField] public Color EvilColor { get; private set; }
        [field: SerializeField] public GameObject Fence { get; private set; }
        [field: SerializeField] public GameObject Fire { get; private set; }
    }
}