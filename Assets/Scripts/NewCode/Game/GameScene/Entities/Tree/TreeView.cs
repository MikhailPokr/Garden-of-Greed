using System;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Garden
{
    public class TreeView : EntityView
    {
        [SerializeField] private SpriteRenderer _treeSpriteRenderer;
        [SerializeField] private SpriteRenderer _crownSpriteRenderer;
        [SerializeField] private BoxCollider2D _collider;
        
        private TreeData _treeData;
        public override IEntityData EntityData => _treeData;
        public override EntityType EntityType => EntityType.Tree;

        private TreePalette _treePalette;
        private Color _woodColor;
        private Color _greenColor;
        
        private List<FruitView> _fruits;

        public override void Init(IEntityData entityData, VisualContext context)
        {
            _treePalette = (TreePalette)context.SpecialPalette;
            _treeData = (TreeData)entityData;
            
            base.Init(entityData, context);
            
            _treeData.DryRequest += OnDryRequest;
            _treeData.GrowRequest += OnGrowRequest;
            
            _crownSpriteRenderer.enabled = false;
            
            _treeSpriteRenderer.sortingOrder = _context.SpriteOrder.GetOrder(entityData.Position.Value.y, SpriteType.Tree);
            _crownSpriteRenderer.sortingOrder =  _context.SpriteOrder.GetOrder(entityData.Position.Value.y, SpriteType.Crown);
            
            _woodColor = _treePalette.GetWoodColor(_treeData.TreeGenome.TreeType, _treeData.TreeGenome.WoodColorIndex);
            _greenColor = ApplyDeviation(_context.GeneralPalette.NormalColor, _treeData.TreeGenome.GreenOffset);
            
            _fruits = new();
        }
        
        public override void SetEntity(EntityView entity)
        {
            entity.gameObject.transform.SetParent(_crownSpriteRenderer.transform);
            entity.gameObject.transform.localPosition = GetPosition();
        }
        
        protected override Vector2 GetPosition()
        {
            var rect = _crownSpriteRenderer.sprite.rect;
            
            for (int i = 0; i < 100; i++)
            {
                Vector2Int pos = new Vector2Int(
                    Random.Range((int)rect.xMin, (int)rect.xMax), 
                    Random.Range((int)rect.yMin, (int)rect.yMax)
                );

                Color color = _crownSpriteRenderer.sprite.texture.GetPixel(pos.x, pos.y);
                
                if (color.a < 0.05f)
                    continue;
                
                float localX = (pos.x - rect.x - _crownSpriteRenderer.sprite.pivot.x) /
                               _crownSpriteRenderer.sprite.pixelsPerUnit;
                float localY = (pos.y - rect.y - _crownSpriteRenderer.sprite.pivot.y) /
                               _crownSpriteRenderer.sprite.pixelsPerUnit;

                return new Vector3(localX, localY, 0);
            }
            return Vector2.zero;
        }

        private void OnGrowRequest(int stage)
        {
            if (_colorSequence.isAlive)
                _colorSequence.Stop();
            
            if (stage == -1)
            {
                _crownSpriteRenderer.enabled = true;
                var sprites = _treePalette.GetTreeSprites(_treeData.TreeGenome.TreeType, _treeData.TreeGenome.GrownSpriteIndex);
                _treeSpriteRenderer.color = _woodColor;
                _crownSpriteRenderer.color = _greenColor;
                _treeSpriteRenderer.sprite = sprites.tree;
                _crownSpriteRenderer.sprite = sprites.crown;
                
                _collider.size = _treeSpriteRenderer.sprite.bounds.size;
                _collider.offset = _treeSpriteRenderer.sprite.bounds.center;

                if (!IsColored)
                    PlayBlinkSequence();
            }
            else
            {
                _treeSpriteRenderer.sprite = _treePalette.StageSprites[stage];
                
                if (IsColored) 
                    PlayColorSequence(true);
                else
                    PlayBlinkSequence();
            }
        }

        protected override void PlayBlinkSequence()
        {
            if (IsColored) return;
            
            _colorSequence = Sequence.Create();
            float duration = 0.15f; 
            
            Color targetWoodColor = _treeData.IsSprout ? _greenColor : _woodColor;
            Color targetCrownColor = _greenColor;

            _colorSequence.Group(Tween.Color(_treeSpriteRenderer, targetWoodColor, duration));
            if (!_treeData.IsSprout)
                _colorSequence.Group(Tween.Color(_crownSpriteRenderer, targetCrownColor, duration));

            _colorSequence.Chain(Tween.Color(_treeSpriteRenderer, _context.GeneralPalette.NoColor, duration));
            if (!_treeData.IsSprout)
                _colorSequence.Group(Tween.Color(_crownSpriteRenderer, _context.GeneralPalette.NoColor, duration));
        }
        
        protected override void PlayColorSequence(bool toFullColor)
        {
            _colorSequence = Sequence.Create();
            float duration = 0.15f;

            Color targetWoodColor = toFullColor 
                ? (_treeData.IsSprout ? _greenColor : _woodColor) 
                : _context.GeneralPalette.NoColor;
        
            Color targetCrownColor = toFullColor ? _greenColor : _context.GeneralPalette.NoColor;

            if (_treeSpriteRenderer.color != targetWoodColor)
                _colorSequence.Group(Tween.Color(_treeSpriteRenderer, targetWoodColor, duration));
        
            if (!_treeData.IsSprout && _crownSpriteRenderer.color != targetCrownColor)
                _colorSequence.Group(Tween.Color(_crownSpriteRenderer, targetCrownColor, duration));
        }

        private void OnDryRequest()
        {
            _crownSpriteRenderer.enabled = false;
            
            if (!IsColored) 
                PlayBlinkSequence();
        }

        protected override void OnDestroy()
        {
            _treeData.DryRequest -= OnDryRequest;
            _treeData.GrowRequest -= OnGrowRequest;
            
            base.OnDestroy();
        }
    }
}