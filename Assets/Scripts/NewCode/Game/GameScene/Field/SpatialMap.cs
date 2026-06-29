using System;
using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public class SpatialMap : ISpatialMap
    {
        private readonly FieldOptions _fieldOptions;

        public (Vector2Int boundsX, Vector2Int boundsY) Bounds =>
            (new Vector2Int(_fieldOptions.Bounds.xMin, _fieldOptions.Bounds.xMax),
                new Vector2Int(_fieldOptions.Bounds.yMin, _fieldOptions.Bounds.yMax));
        
        public Dictionary<Vector2Int, EntityType> EntitiesMainCell { get; }
        public Dictionary<Vector2Int, Dictionary<EntityType, int>> EntitiesFreeCell { get; }
        public Dictionary<Vector2Int, EntityType[]> EntitiesSubCell { get; }

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

        private List<EntityType> _mainCellEntity = new List<EntityType>()
        {
            EntityType.Tree,
            EntityType.DeadShoot
        };

        private List<EntityType> _freeCellEntity = new List<EntityType>()
        {
            EntityType.Fruit,
        };

        private List<EntityType> _subCellEntity = new List<EntityType>()
        {
            EntityType.Grass,
            EntityType.Berry
        };
        
        public SpatialMap(
            FieldOptions fieldOptions)
        {
            _fieldOptions = fieldOptions;
            EntitiesMainCell = new Dictionary<Vector2Int, EntityType>();
            EntitiesFreeCell = new  Dictionary<Vector2Int, Dictionary<EntityType, int>>();
        }

        public void OccupyTile(Vector2Int pos, EntityType entity)
        {
            if (_mainCellEntity.Contains(entity))
            {
                EntitiesMainCell[pos] = entity;
            }
            if (_freeCellEntity.Contains(entity))
            {
                if (!EntitiesFreeCell.ContainsKey(pos))
                    EntitiesFreeCell[pos] = new Dictionary<EntityType, int>();
                if (!EntitiesFreeCell[pos].ContainsKey(entity))
                    EntitiesFreeCell[pos][entity] = 0;
                EntitiesFreeCell[pos][entity]++;
            }
        }

        public void OccupySubTile(Vector2Int pos, int subCell, EntityType entity)
        {
            if (_subCellEntity.Contains(entity))
            {
                if (!EntitiesSubCell.ContainsKey(pos))
                    EntitiesSubCell[pos] = new EntityType[6];
                EntitiesSubCell[pos][subCell] = entity;
            }
        }

        public void FreeTile(Vector2Int pos, EntityType entity)
        {
            if (_mainCellEntity.Contains(entity))
            {
                EntitiesMainCell.Remove(pos);
            }
            if (_freeCellEntity.Contains(entity))
            {
                EntitiesFreeCell[pos][entity]--;
            }
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

        public Vector3 GetPoint(IEntityData data)
        {
            if (data is ISubEntity subEntity)
                return GetPoint(subEntity.Position, subEntity.SubPosition);
            return GetPoint(data.Position);
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
        
        public Vector3 GetPoint(Vector2Int position)
        {
            float xOffset = (position.y & 1) * (_fieldOptions.CellWidth / 2f);

            float worldX = (position.x * _fieldOptions.CellWidth) + xOffset;
            float worldY = -position.y * _fieldOptions.RowHeight;

            return _fieldOptions.Center + new Vector3(worldX, worldY, 0);
        }

        public Vector3 GetPoint(Vector2Int position, int subPosition)
        {
            /*if (position != Vector2Int.zero)
                return new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);*/
            
            Vector3 center = GetPoint(position);
            float cw = _fieldOptions.CellWidth;
            float rh = _fieldOptions.RowHeight;

            Vector3 offset = Vector3.zero;
            
            int yBit = position.y & 1;

            switch (subPosition)
            {
                case 0:
                    offset = (center - GetPoint(new Vector2Int(position.x - 2 + yBit, position.y + 3))) / _fieldOptions.SubCellCoefficient;
                    offset.y -= _fieldOptions.SubCellOffsetY;
                    break;
        
                case 1: 
                    offset = (center - GetPoint(new Vector2Int(position.x - 3, position.y))) / _fieldOptions.SubCellCoefficient;
                    break;
        
                case 2:
                    offset = (center - GetPoint(new Vector2Int(position.x - 2 + yBit, position.y - 3))) / _fieldOptions.SubCellCoefficient;
                    offset.y += _fieldOptions.SubCellOffsetY;
                    break;
        
                case 3: 
                    offset = (center - GetPoint(new Vector2Int(position.x + 1 + yBit, position.y - 3))) / _fieldOptions.SubCellCoefficient;
                    offset.y += _fieldOptions.SubCellOffsetY;
                    break;
        
                case 4: 
                    offset = (center - GetPoint(new Vector2Int(position.x + 3, position.y))) / _fieldOptions.SubCellCoefficient;
                    break;
        
                case 5: 
                    offset = (center - GetPoint(new Vector2Int(position.x + 1 + yBit, position.y + 3))) / _fieldOptions.SubCellCoefficient;
                    offset.y -= _fieldOptions.SubCellOffsetY;
                    break;
            }

            return center + offset;
        }

    }
}