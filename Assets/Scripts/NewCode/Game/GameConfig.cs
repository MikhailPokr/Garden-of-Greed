using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Garden/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public GeneralPalette GeneralPalette { get; private set; }
        [field: SerializeField] public PlayerStartOptions StartOptions { get; private set; }
        [field: SerializeField] public SpriteOrderOptions SpriteOrderOptions { get; private set; }
        [field: SerializeField] public MutationOptions MutationOptions { get; private set; }
        
        [field: SerializeField] public Field FieldPrefab { get; private set; }
        [field: SerializeField] public FieldOptions FieldOptions { get; private set; }
        
        [field: SerializeField] public List<EntityBundle> EntityBundles { get; private set; }
        
        [field: SerializeField] public bool UseSeed { get; private set; }
        [field: SerializeField] public int Seed { get; private set; } 
    }
}