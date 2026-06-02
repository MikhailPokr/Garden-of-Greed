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

        public override void Init(IEntityData entityData, SpriteOrderOptions spriteOrderOptions, IPalette specialPalette, Field field, Vector2Int position)
        {
            _treePalette = (TreePalette)specialPalette;
            _treeData = (TreeData)entityData;
            base.Init(entityData, spriteOrderOptions, specialPalette, field, position);
            
            _treeData.DryRequest += OnDryRequest;
            _treeData.GrowRequest += OnGrowRequest;
            
            _crownSpriteRenderer.enabled = false;
            
            _treeSpriteRenderer.sortingOrder = _spriteOrderOptions.GetOrder(position.y, SpriteType.Tree);
            _crownSpriteRenderer.sortingOrder =  _spriteOrderOptions.GetOrder(position.y, SpriteType.Crown);
        }

        private void OnGrowRequest(int stage)
        {
            if (stage == -1)
            {
                _crownSpriteRenderer.enabled = true;
                _treeSpriteRenderer.sprite = _treePalette.TreeSprites[_treeData.DataConfig.GrownSpriteIndex];
                _crownSpriteRenderer.sprite = _treePalette.CrownSprites[_treeData.DataConfig.GrownSpriteIndex];
                
                _collider.size = _treeSpriteRenderer.sprite.bounds.size;
                _collider.offset = _treeSpriteRenderer.sprite.bounds.center;
                return;
            }
            
            _treeSpriteRenderer.sprite = _treePalette.StageSprites[stage];
        }

        private void OnDryRequest()
        {
            _crownSpriteRenderer.enabled = false;
        }

        protected override void OnSetColor(bool color)
        {
            throw new System.NotImplementedException();
        }
    }
}