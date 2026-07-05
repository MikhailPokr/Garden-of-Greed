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
            
            _fruitSpriteRenderer.sortingOrder = _context.SpriteOrder.GetOrder(entityData.Position.y, SpriteType.Fruit);

            _color = ApplyDeviation(_fruitPalette.GetColor(_fruitData.TreeGenome.TreeType, _fruitData.TreeGenome.FruitColorIndex),
                _fruitData.DataConfig.ColorOffset);
                
            _fruitSpriteRenderer.sprite = _fruitPalette.GetSprite(_fruitData.TreeGenome.TreeType, _fruitData.TreeGenome.FruitSpriteIndex);
            _fruitSpriteRenderer.color = _color;
            _fruitBoxCollider.size = _fruitSpriteRenderer.sprite.bounds.size;
            _fruitBoxCollider.offset = _fruitSpriteRenderer.sprite.bounds.center;
        }

        protected override void OnCommand(ICommand[] commands)
        {
            foreach (var command in commands)
                switch (command)
                {
                    case MarkChangesCommand:
                        if (IsColored)
                            PlayColorSequence(true);
                        else
                            PlayBlinkSequence();
                        break;
                    case DestroyCommand:
                        Destroy(gameObject);
                        break;
                }
        }

        protected override void OnInteract(InteractionType type, bool fieldSource)
        {
            if (type == InteractionType.Click && fieldSource)
                return;
            
            base.OnInteract(type, fieldSource); 
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

        public void OnPointerClick(PointerEventData eventData) => OnInteract(InteractionType.Click, false);
        public void OnPointerEnter(PointerEventData eventData) => OnInteract(InteractionType.HoverStart, false);
        public void OnPointerExit(PointerEventData eventData) => OnInteract(InteractionType.HoverEnd, false);
    }
}