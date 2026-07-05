using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

namespace Garden
{
    public class GrassView : EntityView
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private GrassData _grassData;
        public override IEntityData EntityData => _grassData;
        public override EntityType EntityType => EntityType.Grass;

        private GrassPalette _grassPalette;
        private Color _grassColor;

        public override void Init(IEntityData entityData, VisualContext context)
        {
            _grassPalette = (GrassPalette)context.SpecialPalette;
            _grassData = (GrassData)entityData;
            
            base.Init(entityData, context);
            
            _spriteRenderer.sortingOrder = _context.SpriteOrder.GetOrder(entityData.Position.y, _context.SpriteOrder.GetGrass(_grassData.SubPosition));
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
                        _grassColor = _context.GeneralPalette.NormalColor;
                        break;
                    case ChangeSpriteCommand spriteCommand:
                        _spriteRenderer.sprite = _grassPalette.GrassSprites[spriteCommand.Value];
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

            Color targetWoodColor = toFullColor ? _grassColor : _context.GeneralPalette.NoColor;

            if (_spriteRenderer.color != targetWoodColor)
                _colorSequence.Group(Tween.Color(_spriteRenderer, targetWoodColor, duration));
        }

        protected override void PlayBlinkSequence()
        {
            if (IsColored) return;
            
            _colorSequence = Sequence.Create();
            float duration = 0.15f;

            Color targetColor = _grassColor;

            _colorSequence.Group(Tween.Color(_spriteRenderer, targetColor, duration));

            _colorSequence.Chain(Tween.Color(_spriteRenderer, _context.GeneralPalette.NoColor, duration));
        }
    }
}