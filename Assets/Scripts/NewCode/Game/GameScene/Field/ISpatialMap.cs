using System;
using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public interface ISpatialMap : IGridMath
    {
        bool IsTileFreeAndValid(Vector2Int pos);
        bool IsTileFreeAndValid(CellData data);
        bool IsTileFreeAndValid(CellData data, params EntityType[] validTypes);
        void OccupyTile(CellData data);
        void FreeTile(CellData data);
        event Action<Vector2Int> MapUpdated;
        List<CellData> CellDataList { get; }
        (Vector2Int boundsX, Vector2Int boundsY) Bounds { get; }
    }
}