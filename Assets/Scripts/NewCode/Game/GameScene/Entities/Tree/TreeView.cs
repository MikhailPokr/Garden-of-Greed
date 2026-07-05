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
        
        private TreeData _treeData;
        public override IEntityData EntityData => _treeData;
        public override EntityType EntityType => EntityType.Tree;

        private TreePalette _treePalette;
        private Color _treeColor;
        private Color _greenColor;

        public override void Init(IEntityData entityData, VisualContext context)
        {
            _treePalette = (TreePalette)context.SpecialPalette;
            _treeData = (TreeData)entityData;
            
            base.Init(entityData, context);
            
            _treeData.CommandRequest += OnCommand;
            
            _crownSpriteRenderer.enabled = false;
            
            _treeSpriteRenderer.sortingOrder = _context.SpriteOrder.GetOrder(entityData.Position.y, SpriteType.Tree);
            _crownSpriteRenderer.sortingOrder =  _context.SpriteOrder.GetOrder(entityData.Position.y, SpriteType.Crown);
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
                    case ChangeSpriteCommand spriteCommand:
                        switch (spriteCommand.Value)
                        {
                            case (int)TreeSpriteCommandsLegend.Pit:
                                _crownSpriteRenderer.enabled = false;
                                _treeSpriteRenderer.sprite = _treePalette.Pit;
                                break;
                            case (int)TreeSpriteCommandsLegend.GrownTree:
                                _crownSpriteRenderer.enabled = true;
                                var sprites = _treePalette.GetTreeSprites(_treeData.TreeGenome.TreeType, _treeData.TreeGenome.GrownSpriteIndex);
                                _treeSpriteRenderer.sprite = sprites.tree;
                                _crownSpriteRenderer.sprite = sprites.crown;
                                break;
                            case (int)TreeSpriteCommandsLegend.DeadShoot:
                                _crownSpriteRenderer.enabled = false;
                                _treeSpriteRenderer.sprite = _treePalette.DeadShoot;
                                break;
                            default:
                                _crownSpriteRenderer.enabled = false;
                                _treeSpriteRenderer.sprite = _treePalette.StageSprites[spriteCommand.Value];
                                break;
                        }
                        break;
                    case ChangeColorCommand colorCommand:
                        switch (colorCommand.Value)
                        {
                            case (int)TreeColorCommandsLegend.Pit:
                                _treeColor = _treePalette.PitColor;
                                break;
                            case (int)TreeColorCommandsLegend.Sprout:
                                _treeColor = ApplyDeviation(_context.GeneralPalette.NormalColor, _treeData.TreeGenome.GreenOffset);
                                break;
                            case (int)TreeColorCommandsLegend.Tree:
                                _greenColor = ApplyDeviation(_context.GeneralPalette.NormalColor, _treeData.TreeGenome.GreenOffset);
                                _treeColor = _treePalette.GetWoodColor(_treeData.TreeGenome.TreeType, _treeData.TreeGenome.WoodColorIndex);
                                break;
                            case (int)TreeColorCommandsLegend.DeadShot:
                                _treeColor = _treePalette.GetWoodColor(_treeData.TreeGenome.TreeType, _treeData.TreeGenome.WoodColorIndex);
                                break;
                        }
                        break;
                    case DryCommand:
                        _crownSpriteRenderer.enabled = false;
                        break;
                    case DestroyCommand:
                        Destroy(gameObject);
                        break;
                }
            }
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

        protected override void PlayBlinkSequence()
        {
            if (IsColored) return;
            
            _colorSequence = Sequence.Create();
            float duration = 0.15f;

            Color targetWoodColor = _treeColor;
            Color targetCrownColor = _greenColor;

            _colorSequence.Group(Tween.Color(_treeSpriteRenderer, targetWoodColor, duration));
            _colorSequence.Group(Tween.Color(_crownSpriteRenderer, targetCrownColor, duration));

            _colorSequence.Chain(Tween.Color(_treeSpriteRenderer, _context.GeneralPalette.NoColor, duration));
            _colorSequence.Group(Tween.Color(_crownSpriteRenderer, _context.GeneralPalette.NoColor, duration));
        }
        
        protected override void PlayColorSequence(bool toFullColor)
        {
            _colorSequence = Sequence.Create();
            float duration = 0.15f;

            Color targetWoodColor = toFullColor ? _treeColor : _context.GeneralPalette.NoColor;
            Color targetCrownColor = toFullColor ? _greenColor : _context.GeneralPalette.NoColor;

            if (_treeSpriteRenderer.color != targetWoodColor)
                _colorSequence.Group(Tween.Color(_treeSpriteRenderer, targetWoodColor, duration));
        
            if (_crownSpriteRenderer.color != targetCrownColor)
                _colorSequence.Group(Tween.Color(_crownSpriteRenderer, targetCrownColor, duration));
        }
    }
}