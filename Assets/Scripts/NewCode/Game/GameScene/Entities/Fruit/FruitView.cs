using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Garden
{
    public class FruitView : EntityView, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private SpriteRenderer _fruitSpriteRenderer;
        [SerializeField] private BoxCollider2D _fruitBoxCollider;
        
        private FruitData _fruitData;
        public override IEntityData EntityData => _fruitData;
        public override EntityType EntityType => EntityType.Fruit;
        
        private FruitPalette _fruitPalette;
        private Color _color;

        public override void Init(IEntityData entityData, VisualContext context)
        {
            _fruitPalette = (FruitPalette)context.SpecialPalette;
            _fruitData = (FruitData)entityData;
            
            base.Init(entityData, context);
            
            _fruitSpriteRenderer.sortingOrder = _context.SpriteOrder.GetOrder(entityData.Position.Value.y, SpriteType.Fruit);

            _selected = _context.Field.CurrentHoverPosition == entityData.Position;
            _color = ApplyDeviation(_fruitPalette.GetColor(_fruitData.TreeGenome.TreeType, _fruitData.TreeGenome.FruitColorIndex),
                _fruitData.DataConfig.ColorOffset);
                
            _fruitSpriteRenderer.sprite = _fruitPalette.GetSprite(_fruitData.TreeGenome.TreeType, _fruitData.TreeGenome.FruitSpriteIndex);
            _fruitSpriteRenderer.color = _color;
            _fruitBoxCollider.size = _fruitSpriteRenderer.sprite.bounds.size;
            _fruitBoxCollider.offset = _fruitSpriteRenderer.sprite.bounds.center;
            
            if (IsColored)
                PlayColorSequence(true);
            else
                PlayBlinkSequence();
        }
        
        protected override void OnInteract(InteractionType type)
        {
            if (type == InteractionType.Click)
                return;
            
            base.OnInteract(type); 
        }

        protected override void PlayBlinkSequence()
        {
            if (IsColored) return;
            
            _colorSequence = Sequence.Create();
            float duration = 0.15f;

            _colorSequence.Group(Tween.Color(_fruitSpriteRenderer, _color, duration));
            _colorSequence.Chain(Tween.Color(_fruitSpriteRenderer, _context.GeneralPalette.NoColor, duration));
        }
        
        protected override void PlayColorSequence(bool toFullColor)
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

        public void OnPointerClick(PointerEventData eventData) => base.OnInteract(InteractionType.Click);
        public void OnPointerEnter(PointerEventData eventData) => OnInteract(InteractionType.HoverStart);
        public void OnPointerExit(PointerEventData eventData) => OnInteract(InteractionType.HoverEnd);
    }
}