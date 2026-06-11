using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public interface IGridMath
    {
        Vector3 GetPoint(Vector2Int position);
        Vector2Int GetPosition(Vector3 worldPosition);
        List<Vector2Int> GetNeighbors(Vector2Int position);
    }
}