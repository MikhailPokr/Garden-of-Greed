
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Garden
{
    public class GeoMap
    {
        private readonly VisualContext _visualContext;

        private float _timer;
        private readonly Dictionary<Vector2Int, int> _cells;
        public Dictionary<Vector2Int, int> Map { get; private set; }

        public GeoMap(VisualContext visualContext)
        {
            _visualContext = visualContext;
            _cells = new Dictionary<Vector2Int, int>();
            Map = new Dictionary<Vector2Int, int>();
            
            _visualContext.SpatialMap.MapUpdated += OnSpatialMapUpdated;
            SignalBus<FieldClickSignal>.OnEvent += (signal) =>
            {
                if (signal.InteractionType != InteractionType.Click)
                    return;
                Debug.Log($"{signal.Position.x}:{signal.Position.y} | " + Map.GetValueOrDefault(signal.Position, 0));
            };
        }

        private void OnSpatialMapUpdated(Vector2Int position)
        {
            int tres = _visualContext.SpatialMap.CellDataList.FindAll(x => x.Position == position && x.CellType == CellType.Main).Count;
            int grass = _visualContext.SpatialMap.CellDataList.FindAll(x => x.Position == position && x.CellType == CellType.Sub).Sum(x => x.Value);
            _cells[position] = tres + grass;
            Calculate();
        }

        private void Calculate()
        {
            (Vector2Int boundsX, Vector2Int boundsY) bounds = _visualContext.SpatialMap.Bounds;
            for (int x = bounds.boundsX.x; x < bounds.boundsX.y; x++)
            {
                for (int y = bounds.boundsY.x; y < bounds.boundsY.y; y++)
                {
                    var pos = new Vector2Int(x, y);
                    Map[pos] = _cells.GetValueOrDefault(pos, 0);
                    var n = _visualContext.SpatialMap.GetNeighbors(pos);
                    foreach (var i in n)
                    {
                        Map[pos] += _cells.GetValueOrDefault(i, 0) / 2;
                    }
                }
            }
        }
    }
}

