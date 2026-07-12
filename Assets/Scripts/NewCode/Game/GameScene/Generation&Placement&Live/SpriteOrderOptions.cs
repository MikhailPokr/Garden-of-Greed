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
        private readonly Dictionary<int, SpriteType> _berrySprites = new Dictionary<int, SpriteType>()
        {
            { 0, SpriteType.BerryLine1 },
            { 1, SpriteType.BerryLine1 },
            { 2, SpriteType.BerryLine2 },
            { 3, SpriteType.BerryLine2 },
            { 4, SpriteType.BerryLine3 },
            { 5, SpriteType.BerryLine3 },
        };
        public SpriteType GetGrass(int subCell) => _grassSprites[subCell];
        public (SpriteType leaves, SpriteType berry) GetBerry(int subCell) => (_grassSprites[subCell], _berrySprites[subCell]);
        public int GetOrder(int Y, SpriteType spriteType) => Y * SpriteTypes.Count + SpriteTypes.IndexOf(spriteType);
    }
}