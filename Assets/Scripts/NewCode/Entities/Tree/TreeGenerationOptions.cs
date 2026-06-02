using System;
using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "TreeGenerationOptions", menuName = "Garden/Options/Entities/TreeGenerationOptions")]
    public class TreeGenerationOptions : BaseGenerationOptions
    {
        [Header("General")]
        [SerializeField] private Vector2Int _dryWoodCostRange;
        [SerializeField, Min(1)] private Vector2Int _growthLastStageRange;
        [SerializeField] private Vector2 _greenOffsetRange;

        [Header("Normal")]
        [SerializeField] private Vector2Int _autoBreedCountRange;
        [SerializeField] private Vector2Int _autoBreedMutationPercentRange;
        [SerializeField, Min(0)] private Vector2 _stageTimeRange;
        [SerializeField, Min(0)] private Vector2Int _maxStageRange;
        [SerializeField] private Vector2Int _woodCostRange;

        [Header("Fruit")]
        [SerializeField, Range(0, 100)] private int _fruitChance;
        [SerializeField, Min(0)] private Vector2 _fruitStageTimeRange;
        [SerializeField, Min(0)] private Vector2Int _lastFruitStageRange;
        [SerializeField, Min(0)] private Vector2Int _fruitMaxStageRange;
        [SerializeField] private Vector2Int _fruitWoodCostRange;
        [SerializeField] private Vector2Int _baseFruitCostRange;

        [Header("Evil")]
        [SerializeField, Range(0, 100)] private int _evilChance;
        [SerializeField] private Vector2Int _evilAutoBreedCountRange;
        [SerializeField] private Vector2Int _evilBreedMutationPercentRange;
        [SerializeField, Min(0)] private Vector2 _evilStageTimeRange;
        [SerializeField, Min(0)] private Vector2Int _evilLastFruitStageRange;
        [SerializeField, Min(0)] private Vector2Int _evilMaxStageRange;
        [SerializeField] private Vector2Int _evilWoodCostRange;
        [SerializeField] private Vector2Int _evilBaseFruitCostRange;

        public Vector2Int GetGrowthLastStageRange(TreeType treeType) => _growthLastStageRange;
        public Vector2 GetGreenOffsetRange() => _greenOffsetRange;
        public Vector2 GetStageTimeRange(TreeType treeType) => treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => _evilStageTimeRange,
            _ when (treeType & TreeType.Fruit) != 0 => _fruitStageTimeRange,
            _ => _stageTimeRange
        };
        public Vector2Int GetMaxStageRange(TreeType treeType) => treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => _evilMaxStageRange,
            _ when (treeType & TreeType.Fruit) != 0 => _fruitMaxStageRange,
            _ => _maxStageRange
        };

        public Vector2Int GetWoodCostRange(TreeType treeType) => treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => _evilWoodCostRange,
            _ when (treeType & TreeType.Fruit) != 0 => _fruitWoodCostRange,
            _ => _woodCostRange
        };
        public Vector2Int GetAutoBreedMutationPercentRange(TreeType treeType) =>  treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => _evilBreedMutationPercentRange,
            _ => _autoBreedMutationPercentRange
        };
        public Vector2Int GetAutoBreedCountRange(TreeType treeType) => treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => _evilAutoBreedCountRange,
            _ => _autoBreedCountRange
        };
        public Vector2Int GetDryWoodCostRange() => _dryWoodCostRange;

        public Vector2Int GetBaseFruitCostRange(TreeType treeType) => treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => _evilLastFruitStageRange,
            _ => _lastFruitStageRange
        };
        public Vector2Int GetLastFruitStageRange(TreeType treeType) => treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => _evilLastFruitStageRange,
            _ => _lastFruitStageRange
        };

        public List<TreeTypeConfig> GetChances() => new List<TreeTypeConfig>()
        {
            new(TreeType.Fruit, ParamType.IsFruit, _fruitChance),
            new(TreeType.Evil, ParamType.IsEvil, _evilChance)
        };
    }
}