using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "TreeGenerationOptions", menuName = "Garden/Options/TreeGenerationOptions")]
    public class TreeGenerationOptions : ScriptableObject
    {
        //General
        [field: SerializeField] public int MaximumStagesReduction { get; private set; }
        [field: SerializeField] public Vector2Int DryWoodCostRange { get; private set; }
        //Normal
        [field: SerializeField] public float AutoBreedChance { get; private set; }
        [field: SerializeField] public Vector2Int AutoBreedCountRange { get; private set; }
        [field: SerializeField] public Vector2 AutoBreedMutationRange { get; private set; }
        [field: SerializeField] public Vector2 NormalStageTimeRange { get; private set; }
        [field: SerializeField] public Vector2Int NormalMaxStageRange  { get; private set; }
        [field: SerializeField] public Vector2Int NormalWoodCostRange { get; private set; }
        //Fruit
        [field: SerializeField] public float FruitChance{ get; private set; }
        [field: SerializeField] public Vector2 FruitStageTimeRange { get; private set; }
        [field: SerializeField] public Vector2Int LastFruitStageRange { get; private set; }
        [field: SerializeField] public Vector2Int FruitMaxStageRange  { get; private set; }
        [field: SerializeField] public Vector2Int FruitWoodCostRange { get; private set; }
        [field: SerializeField] public Vector2Int BaseFruitCostRange { get; private set; }
        //Evil
        [field: SerializeField] public float EvilChance { get; private set; }
        [field: SerializeField] public Vector2 EvilStageTimeRange { get; private set; }
        [field: SerializeField] public Vector2Int EvilLastFruitStageRange { get; private set; }
        [field: SerializeField] public Vector2Int EvilMaxStageRange  { get; private set; }
        [field: SerializeField] public Vector2Int EvilWoodCostRange { get; private set; }
        [field: SerializeField] public Vector2Int EvilBaseFruitCostRange { get; private set; }
        
    }
}