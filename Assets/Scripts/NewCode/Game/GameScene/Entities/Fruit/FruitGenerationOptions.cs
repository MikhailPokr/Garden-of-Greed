using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "FruitGenerationOptions", menuName = "Garden/Options/Entities/FruitGenerationOptions")]
    public class FruitGenerationOptions : BaseGenerationOptions
    {
        [SerializeField] private Vector2 _colorOffsetRange;
        [Header("Normal")]
        [SerializeField] private Vector2 _fruitCostMultiplierRange;
        [SerializeField] private Vector2Int _fruitCountPerStageRange;
        [SerializeField] private Vector2 _rottingTimeRange;
        [SerializeField, Range(0,1)] private float _growUpChanceRange;
        [SerializeField] private Vector2Int _startQualityRange;
        [SerializeField] private Vector2Int _lifeRegenerationRange;
        [Header("Evil")]
        [SerializeField] private Vector2 _evilFruitCostMultiplierRange;
        [SerializeField] private Vector2Int _evilFruitCountPerStageRange;
        [SerializeField] private Vector2 _evilRottingTimeRange;
        [SerializeField, Range(0,1)] private float _evilGrowUpChanceRange;
        [SerializeField] private Vector2Int _evilStartQualityRange;
        [SerializeField] private Vector2Int _evilLifeRegenerationRange;
        
        public Vector2Int GetCountPerStageRange(TreeType treeType) => treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => _fruitCountPerStageRange,
            _ => _evilFruitCountPerStageRange
        };
        
        public Vector2 GatFruitCostMultiplierRange(TreeType treeType) => treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => _fruitCostMultiplierRange,
            _ => _evilFruitCostMultiplierRange
        };

        public Vector2 GetRottingTimeRange(TreeType treeType) => treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => _evilRottingTimeRange,
            _ => _rottingTimeRange
        };

        public float GetGrowUpChance(TreeType treeType) => treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => _evilGrowUpChanceRange,
            _ => _growUpChanceRange
        };
        
        public Vector2Int GetStartQualityRange(TreeType treeType) => treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => _evilStartQualityRange,
            _ => _startQualityRange
        };

        public Vector2Int GetLifeRegenerationRange(TreeType treeType) => treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => _evilLifeRegenerationRange,
            _ => _lifeRegenerationRange
        };
        
        public Vector2 GetColorOffsetRange() => _colorOffsetRange;
    }
}