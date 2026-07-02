using UnityEngine;

namespace Garden
{
    public class CellData
    {
        public readonly CellType CellType;
        public readonly EntityType EntityType;
        public readonly Vector2Int Position; 
        public readonly int SubCell;
        public int Value;

        public CellData(CellType cellType, EntityType entityType, Vector2Int position)
        {
            CellType = cellType;
            EntityType = entityType;
            Position = position;
            SubCell = -1;
            Value = 0;
        }
        
        public CellData(CellType cellType, EntityType entityType, Vector2Int position, int subCell)
        {
            CellType = cellType;
            EntityType = entityType;
            Position = position;
            SubCell = subCell;
            Value = 0;
        }

        public bool Compare(CellData other, bool nullIsAll = false)
        {
            if (nullIsAll)
            {
                return
                    EntityType == other.EntityType &&
                    Position == other.Position &&
                    SubCell == other.SubCell;
            }
            return
                CellType == other.CellType &&
                EntityType == other.EntityType &&
                Position == other.Position &&
                SubCell == other.SubCell;
        }
    }
}