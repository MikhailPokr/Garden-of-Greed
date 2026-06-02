using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "SpriteOrderOption", menuName = "Garden/Options/SpriteOrderOption")]
    public class SpriteOrderOptions : ScriptableObject
    {
        [field: SerializeField] public List<SpriteType> SpriteTypes { get; set; }

        public int GetOrder(int Y, SpriteType spriteType) => Y * SpriteTypes.Count + SpriteTypes.IndexOf(spriteType);
    }
}