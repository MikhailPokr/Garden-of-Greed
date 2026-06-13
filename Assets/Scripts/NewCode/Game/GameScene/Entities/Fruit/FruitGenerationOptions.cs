using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "FruitGenerationOptions", menuName = "Garden/Options/Entities/FruitGenerationOptions")]
    public class FruitGenerationOptions : BaseGenerationOptions
    {
        [Header("Normal")]
        [SerializeField] private Vector2Int _baseFruitCostRange;
        [SerializeField] private Vector2Int _fruitCountPerStageRange;
        [Header("Evil")]
        [SerializeField] private Vector2Int _evilBaseFruitCostRange;
        [SerializeField] private Vector2Int _evilFruitCountPerStageRange;
        
        public Vector2Int GetCountPerStageRange(TreeType treeType) => treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => _fruitCountPerStageRange,
            _ => _evilFruitCountPerStageRange
        };
        
        public Vector2Int GatFruitCostRange(TreeType treeType) => treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => _baseFruitCostRange,
            _ => _evilBaseFruitCostRange
        };
    }
}