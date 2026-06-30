using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "SpriteOrderOption", menuName = "Garden/Options/SpriteOrderOption")]
    public class SpriteOrderOptions : ScriptableObject
    {
        [field: SerializeField] public List<SpriteType> SpriteTypes { get; set; }

        private readonly Dictionary<int, SpriteType> _grassSprites = new Dictionary<int, SpriteType>()
        {
            { 0, SpriteType.GrassLine1 },
            { 1, SpriteType.GrassLine2 },
            { 2, SpriteType.GrassLine3 },
            { 3, SpriteType.GrassLine3 },
            { 4, SpriteType.GrassLine2 },
            { 5, SpriteType.GrassLine1 },
        };
        public SpriteType GetGrass(int subCell) => _grassSprites[subCell];
        public int GetOrder(int Y, SpriteType spriteType) => Y * SpriteTypes.Count + SpriteTypes.IndexOf(spriteType);
    }
}