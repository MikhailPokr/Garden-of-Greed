using System;
using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public class SpatialMap : ISpatialMap
    {   
        private readonly FieldOptions _fieldOptions;
        
        public Dictionary<Vector2Int, EntityType> EntitiesMainCell { get; }
        
        private static readonly Vector2Int[][] NeighborDirections = new Vector2Int[][]
        {
            new Vector2Int[] 
            {
                new Vector2Int(0, -1),
                new Vector2Int(0, 1),
                new Vector2Int(0, 2),
                new Vector2Int(-1, 1),
                new Vector2Int(-1, -1),
                new Vector2Int(0, -2) 
            },
            new Vector2Int[] 
            {
                new Vector2Int(1, -1), 
                new Vector2Int(1, 1), 
                new Vector2Int(0, 2), 
                new Vector2Int(0, 1), 
                new Vector2Int(0, -1), 
                new Vector2Int(0, -2) 
            }
        };
        
        public SpatialMap(
            FieldOptions fieldOptions)
        {
            _fieldOptions = fieldOptions;
            EntitiesMainCell = new Dictionary<Vector2Int, EntityType>();
        }

        public void OccupyTile(Vector2Int pos, EntityType entity)
        {
            EntitiesMainCell[pos] = entity;
        }

        public void FreeTile(Vector2Int pos)
        {
            EntitiesMainCell.Remove(pos);
        }

        public bool IsTileFreeAndValid(Vector2Int pos)
        {
            return !EntitiesMainCell.ContainsKey(pos) && _fieldOptions.Bounds.Contains(pos);
        }
        
        public List<Vector2Int> GetNeighbors(Vector2Int position)
        {
            int parity = position.y & 1; 
            
            var list = new List<Vector2Int>();

            for (int i = 0; i < 6; i++)
            {
                Vector2Int neighbor = position + NeighborDirections[parity][i];
        
                if (!_fieldOptions.Bounds.Contains(neighbor))
                    continue; 
            
                list.Add(neighbor);
            }

            return list;
        }
        
        public Vector3 GetPoint(Vector2Int position)
        {
            float xOffset = (position.y & 1) * (_fieldOptions.CellWidth / 2f);

            float worldX = (position.x * _fieldOptions.CellWidth) + xOffset;
            float worldY = -position.y * _fieldOptions.RowHeight;

            return _fieldOptions.Center + new Vector3(worldX, worldY, 0);
        }

        public Vector2Int GetPosition(Vector3 worldPosition)
        {
            Vector3 localPoint = worldPosition - _fieldOptions.Center;
    
            int roughY = Mathf.RoundToInt(-localPoint.y / _fieldOptions.RowHeight);
            float roughXOffset = (roughY & 1) * (_fieldOptions.CellWidth / 2f);
            int roughX = Mathf.RoundToInt((localPoint.x - roughXOffset) / _fieldOptions.CellWidth);

            Vector2Int bestCell = new Vector2Int(roughX, roughY);
            float minSqrDistance = float.MaxValue;

            for (int y = roughY - 1; y <= roughY + 1; y++)
            {
                for (int x = roughX - 1; x <= roughX + 1; x++)
                {
                    Vector2Int currentCell = new Vector2Int(x, y);
            
                    Vector3 cellCenter = GetPoint(currentCell); 
            
                    float sqrDist = (worldPosition - cellCenter).sqrMagnitude;

                    if (sqrDist < minSqrDistance)
                    {
                        minSqrDistance = sqrDist;
                        bestCell = currentCell;
                    }
                }
            }

            return bestCell;
        }
    }
}