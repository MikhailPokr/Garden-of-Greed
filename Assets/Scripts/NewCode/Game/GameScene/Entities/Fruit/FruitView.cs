using UnityEngine;

namespace Garden
{
    public class FruitView : EntityView
    {
        [SerializeField] private SpriteRenderer _fruitSprite;
        public override IEntityData EntityData { get; }
        public override EntityType EntityType => EntityType.Fruit;
        protected override Vector2 GetPosition()
        {
            throw new System.NotImplementedException();
        }

        public float Radius => Mathf.Max(_fruitSprite.bounds.max.x,  _fruitSprite.bounds.max.y);
    }
}