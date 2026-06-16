using PrimeTween;
using TMPro;
using UnityEngine;

namespace Garden
{
    public class FruitView : EntityView
    {
        [SerializeField] private SpriteRenderer _fruitSpriteRenderer;
        private FruitData _fruitData;
        public override IEntityData EntityData => _fruitData;
        public override EntityType EntityType => EntityType.Fruit;
        
        private FruitPalette _fruitPalette;
        private Sequence _colorSequence;
        private bool _selected;
        private bool _colored;
        private bool IsColored => _colored || _selected;
        private Color _color;

        public override void Init(IEntityData entityData, VisualContext context)
        {
            _fruitPalette = (FruitPalette)context.SpecialPalette;
            _fruitData = (FruitData)entityData;
            base.Init(entityData, context);
            
            SignalBus<ColorModeChangedSignal>.OnEvent += OnColorModeChanged;
            
            _fruitSpriteRenderer.sortingOrder = _context.SpriteOrder.GetOrder(entityData.Position.Value.y, SpriteType.Fruit);

            _colored = context.Color;
            _color = ApplyDeviation(_fruitPalette.GetColor(_fruitData.TreeGenome.TreeType, _fruitData.TreeGenome.FruitColorIndex),
                _fruitData.DataConfig.ColorOffset);
            _fruitSpriteRenderer.sprite = _fruitPalette.GetSprite(_fruitData.TreeGenome.TreeType, _fruitData.TreeGenome.FruitSpriteIndex);
            _fruitSpriteRenderer.color = _color;
        }
        
        protected override void OnInteract(InteractionType type)
        {
            _selected = type switch
            {
                InteractionType.HoverStart => true,
                InteractionType.HoverEnd => false,
                _ => _selected
            };
            PlayColorSequence(IsColored);
            
            base.OnInteract(type);
        }

        private void OnColorModeChanged(ColorModeChangedSignal signal)
        {
            _colored = signal.IsColored;
    
            if (_colorSequence.isAlive)
                _colorSequence.Stop();
        
            PlayColorSequence(signal.IsColored);
        }
        
        private void PlayColorSequence(bool toFullColor)
        {
            _colorSequence = Sequence.Create();
            float duration = 0.15f;
        
            Color targetColor = toFullColor ? _color : _context.GeneralPalette.NoColor;

            if (_fruitSpriteRenderer.color != targetColor)
                _colorSequence.Group(Tween.Color(_fruitSpriteRenderer, targetColor, duration));
        }

        private void OnDrop()
        {
            
        }
    }
}