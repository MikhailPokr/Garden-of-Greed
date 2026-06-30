
using Garden;
using UnityEngine;

public struct MowCellSignal : ISignal
{
    public readonly Vector2Int Position;
    
    public MowCellSignal(Vector2Int position)
    {
        Position = position;
    }
}