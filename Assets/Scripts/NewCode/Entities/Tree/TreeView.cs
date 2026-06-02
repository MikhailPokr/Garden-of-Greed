using System;
using System.Threading.Tasks;
using PrimeTween;
using UnityEngine;

namespace Garden
{
    public class TreeView : EntityView
    {
        [SerializeField] private SpriteRenderer _treeSpriteRenderer;
        [SerializeField] private SpriteRenderer _crownSpriteRenderer;
        [SerializeField] private BoxCollider2D _collider;
        private TreeData _treeData;
        public override IEntityData EntityData => _treeData;
        
        private TreePalette _treePalette;
        private Sequence _colorSequence;
        private bool _colored;
        private Color _woodColor;
        private Color _greenColor;

        public override void Init(IEntityData entityData, VisualContext context, Vector2Int position)
        {
            _treePalette = (TreePalette)context.SpecialPalette;
            _treeData = (TreeData)entityData;
            base.Init(entityData, context, position);
            
            _treeData.DryRequest += OnDryRequest;
            _treeData.GrowRequest += OnGrowRequest;
            SignalBus<ColorModeChangedSignal>.OnEvent += OnColorModeChanged;
            
            _crownSpriteRenderer.enabled = false;
            
            _treeSpriteRenderer.sortingOrder = _context.SpriteOrder.GetOrder(position.y, SpriteType.Tree);
            _crownSpriteRenderer.sortingOrder =  _context.SpriteOrder.GetOrder(position.y, SpriteType.Crown);
            
            _colored = false;
            _woodColor = _treePalette.WoodColors[_treeData.DataConfig.WoodColorIndex];
            _greenColor = ApplyDeviation(_context.GeneralPalette.NormalColor, _treeData.DataConfig.GreenOffset);
        }
        
        private void OnColorModeChanged(ColorModeChangedSignal signal)
        {
            _colored = signal.IsColored;
    
            if (_colorSequence.isAlive)
                _colorSequence.Stop();
        
            PlayColorSequence(signal.IsColored);
        }

        private void OnGrowRequest(int stage)
        {
            if (_colorSequence.isAlive)
                _colorSequence.Stop();
            
            
            if (stage == -1)
            {
                _crownSpriteRenderer.enabled = true;
                _treeSpriteRenderer.sprite = _treePalette.TreeSprites[_treeData.DataConfig.GrownSpriteIndex];
                _crownSpriteRenderer.sprite = _treePalette.CrownSprites[_treeData.DataConfig.GrownSpriteIndex];
                
                _collider.size = _treeSpriteRenderer.sprite.bounds.size;
                _collider.offset = _treeSpriteRenderer.sprite.bounds.center;

                if (_colored)
                {
                    _treeSpriteRenderer.color = _woodColor;
                    _crownSpriteRenderer.color = _greenColor;
                }
                else
                {
                    PlayBlinkSequence();
                }
            }
            else
            {
                _treeSpriteRenderer.sprite = _treePalette.StageSprites[stage];
                
                if (_colored) 
                    PlayColorSequence(true);
                else
                    PlayBlinkSequence();
            }

        }

        private void PlayBlinkSequence()
        {
            if (_colored)
                return;
            
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
        
        private void PlayColorSequence(bool toFullColor)
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
            
            if (!_colored) 
                PlayBlinkSequence();
                
        }
    }
}