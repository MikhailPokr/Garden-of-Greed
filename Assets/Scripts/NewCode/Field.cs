using System;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Garden
{
    public class Field : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _cellWidth;
        [SerializeField] private float _rowHeight;
        [SerializeField] private Vector3 _center;
        
        private Camera _mainCamera;
        private FieldManager _fieldManager;
        private Sequence _colorSequence;
        private RectInt _bounds;

        private InputAction _pointer;

        private Vector2Int _lastPosition;
        public event Action<InteractionType, Vector2Int> FieldInteract; 
        
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


        public void Init(FieldManager fieldManager, RectInt bounds)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _mainCamera = Camera.main;

            _spriteRenderer.drawMode = SpriteDrawMode.Tiled;
            
            _fieldManager = fieldManager;
            _bounds =  bounds;
            
            _pointer = InputSystem.actions.FindAction("Point");
            
            SignalBus<ColorModeChangedSignal>.OnEvent += OnColorChanged;
        }

        public void UpdateLogic()
        {
            Vector2Int position = GetPosition(_mainCamera.ScreenToWorldPoint(_pointer.ReadValue<Vector2>()));
            if (position != _lastPosition)
            {
                FieldInteract?.Invoke(InteractionType.HoverEnd, _lastPosition);
                FieldInteract?.Invoke(InteractionType.HoverStart, position);
            }
            _lastPosition = position;
        }

        private void OnColorChanged(ColorModeChangedSignal signal)
        {
            PlayColorSequence(signal.IsColored);
        }
        
        private void PlayColorSequence(bool toFullColor)
        {
            if (_colorSequence.isAlive)
                _colorSequence.Stop();
    
            _colorSequence = Sequence.Create();
            float duration = 0.15f;

            Color targetColor = toFullColor 
                ? _fieldManager.GeneralPalette.NormalColor 
                : _fieldManager.GeneralPalette.NoColor;

            if (_spriteRenderer.color != targetColor)
            {
                _colorSequence.Group(Tween.Color(_spriteRenderer, targetColor, duration));
            }
        }
        

        public Vector3 GetPoint(Vector2Int position)
        {
            float xOffset = (position.y & 1) * (_cellWidth / 2f);

            float worldX = (position.x * _cellWidth) + xOffset;
            float worldY = -position.y * _rowHeight;

            return _center + new Vector3(worldX, worldY, 0);
        }

        public Vector2Int GetPosition(Vector3 worldPosition)
        {
            Vector3 localPoint = worldPosition - _center;
    
            int roughY = Mathf.RoundToInt(-localPoint.y / _rowHeight);
            float roughXOffset = (roughY & 1) * (_cellWidth / 2f);
            int roughX = Mathf.RoundToInt((localPoint.x - roughXOffset) / _cellWidth);

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
        public List<Vector2Int> GetNeighbors(Vector2Int position)
        {
            int parity = position.y & 1; 
            
            var list = new List<Vector2Int>();

            for (int i = 0; i < 6; i++)
            {
                Vector2Int neighbor = position + NeighborDirections[parity][i];
        
                if (!_bounds.Contains(neighbor))
                    continue; 
            
                list.Add(neighbor);
            }

            return list;
        }

        private void OnDrawGizmos()
        {
            
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(GetPoint(new(0, 0)), 0.11f);
            var a = GetNeighbors(new Vector2Int(0, 0));
            foreach (var i in a)
            {
                Gizmos.DrawWireSphere(GetPoint(i), 0.15f);
            }
            
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(GetPoint(new(0, 3)), 0.1f);
            var b = GetNeighbors(new Vector2Int(0, 3));
            foreach (var i in b)
            {
                Gizmos.DrawWireSphere(GetPoint(i), 0.15f);
            }
            
            Gizmos.color = Color.yellow;
            for (int i = -10; i <= 10; i++)
            {
                for (int j = -10; j <= 10; j++)
                {
                    var pos = new Vector2Int(i, j);
                    UnityEditor.Handles.Label(GetPoint(pos), pos.ToString());
                }
            }
            
            
        }

        public void OnPointerClick(PointerEventData eventData) => 
            FieldInteract?.Invoke(InteractionType.Click, 
                GetPosition(_mainCamera.ScreenToWorldPoint(eventData.position)));
    }
}