using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "Palette", menuName = "Game/Palette")]
    public class Palette : ScriptableObject
    {
        [field: SerializeField] public Sprite FieldSprite { get; private set; }
        [Header("Plants")]
        [field: SerializeField] public Sprite PotSprite { get; private set; }
        [field: SerializeField] public List<Sprite> StageSprites { get; private set; }
        [field: SerializeField] public List<Sprite> TreeSprites { get; private set; }
        [field: SerializeField] public List<Sprite> TreeEvilSprites { get; private set; }
        [field: SerializeField] public Sprite DieSprite { get; private set; }
        [Header("Fruits")]
        [field: SerializeField] public List<Sprite> FruitSprites { get; private set; }
        [field: SerializeField] public List<Sprite> FruitEvilSprites { get; private set; }
        [Header("Grass")]
        [field: SerializeField] public List<Sprite> GrassSprites { get; private set; }
        [field: SerializeField] public List<Sprite> BerrySprites { get; private set; }
        [field: SerializeField] public List<Sprite> BerryEvilSprites { get; private set; }
        [Header("Color")]
        [field: SerializeField] public Color NormalColor { get; private set; }
        [field: SerializeField] public Color EvilColor { get; private set; }
    }
}