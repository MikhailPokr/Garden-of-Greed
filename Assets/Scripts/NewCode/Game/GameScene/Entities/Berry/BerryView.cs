using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Garden
{
    public class BerryView : EntityView, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private SpriteRenderer _leavesSpriteRenderer;
        [SerializeField] private SpriteRenderer _berrySpriteRenderer;
        [SerializeField] private BoxCollider2D _collider;
        
        private BerryData _berryData;
        public override IEntityData EntityData => _berryData;
        public override EntityType EntityType => EntityType.Berry;
        
        private BerryPalette _berryPalette;
        private Color _greenColor;
        private Color _berryColor;

        public override void Init(IEntityData entityData, VisualContext context)
        {
            _berryPalette = (BerryPalette)context.SpecialPalette;
            _berryData = (BerryData)entityData;
            
            base.Init(entityData, context);
            
            var order = _context.SpriteOrder.GetOrder(entityData.Position.y, _context.SpriteOrder.GetGrass(_berryData.SubPosition));
            _leavesSpriteRenderer.sortingOrder = order;
            _berrySpriteRenderer.sortingOrder = order + 1;
        }
        
        protected override void OnCommand(ICommand[] commands)
        {
            foreach (var command in commands)
            {
                switch (command)
                {
                    case MarkChangesCommand:
                        if (IsColored)
                            PlayColorSequence(true);
                        else
                            PlayBlinkSequence();
                        break;
                    case ChangeColorCommand:
                        _greenColor = _context.GeneralPalette.NormalColor;
                        _berryColor = _berryPalette.BerryColors[_berryData.DataConfig.ColorIndex]; 
                        break;
                    case ChangeSpriteCommand:
                        _leavesSpriteRenderer.sprite = _berryPalette.LeavesSprites[_berryData.DataConfig.SpriteIndex];
                        _berrySpriteRenderer.sprite = _berryPalette.BerrySprites[_berryData.DataConfig.SpriteIndex];
                        _collider.size = _leavesSpriteRenderer.sprite.bounds.size;
                        _collider.offset = _leavesSpriteRenderer.sprite.bounds.center;
                        break;
                    case DestroyCommand:
                        Destroy(gameObject);
                        break;
                }
            }
        }

        protected override void PlayColorSequence(bool toFullColor)
        {
            _colorSequence = Sequence.Create();
            float duration = 0.15f;

            Color targetLeavesColor = toFullColor ? _greenColor : _context.GeneralPalette.NoColor;
            Color targetBerryColor = toFullColor ? _berryColor : _context.GeneralPalette.NoColor;

            if (_leavesSpriteRenderer.color != targetLeavesColor)
                _colorSequence.Group(Tween.Color(_leavesSpriteRenderer, targetLeavesColor, duration));
        
            if (_berrySpriteRenderer.color != targetBerryColor)
                _colorSequence.Group(Tween.Color(_berrySpriteRenderer, targetBerryColor, duration));
        }

        protected override void PlayBlinkSequence()
        {
            if (IsColored) return;
            
            _colorSequence = Sequence.Create();
            float duration = 0.15f;

            Color targetLeavesColor = _greenColor;
            Color targetBerryColor = _berryColor;

            _colorSequence.Group(Tween.Color(_leavesSpriteRenderer, targetLeavesColor, duration));
            _colorSequence.Group(Tween.Color(_berrySpriteRenderer, targetBerryColor, duration));

            _colorSequence.Chain(Tween.Color(_leavesSpriteRenderer, _context.GeneralPalette.NoColor, duration));
            _colorSequence.Group(Tween.Color(_berrySpriteRenderer, _context.GeneralPalette.NoColor, duration));
        }
        
        public void OnPointerClick(PointerEventData eventData) => OnInteract(InteractionType.Click, false);
        public void OnPointerEnter(PointerEventData eventData) => OnInteract(InteractionType.HoverStart, false);
        public void OnPointerExit(PointerEventData eventData) => OnInteract(InteractionType.HoverEnd, false);
    }
}