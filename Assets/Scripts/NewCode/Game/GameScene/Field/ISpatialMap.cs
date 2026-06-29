using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public interface ISpatialMap : IGridMath
    {
        bool IsTileFreeAndValid(Vector2Int pos);
        void OccupyTile(Vector2Int pos, EntityType entity);
        void FreeTile(Vector2Int pos, EntityType entity);
        void OccupySubTile(Vector2Int pos, int subCell, EntityType entity);
    }
}