using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "GrassGenerationOptions", menuName = "Garden/Options/Entities/GrassGenerationOptions")]
    public class GrassGenerationOptions : BaseGenerationOptions
    {
        [SerializeField] private Vector2 _generationTimeRange;
        
        [SerializeField] private Vector2Int _countPerUseRange;
        [SerializeField] private Vector2 _growTimeRange;
        
        public Vector2 GenerationTimeRange() => _generationTimeRange;
        public Vector2Int CountPerUseRange() => _countPerUseRange;
        public Vector2 GrowTimeRange() => _growTimeRange * 10000;
    }
}