using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "GrassGenerationOptions", menuName = "Garden/Options/Entities/GrassGenerationOptions")]
    public class GrassGenerationOptions : BaseGenerationOptions
    {
        [SerializeField] private Vector2 _generationTimeRange;
        [SerializeField] private Vector2Int _countPerUseRange;
        
        public Vector2 GetGenerationTimeRange() => _generationTimeRange;
        public Vector2Int CountPerUseRange() => _countPerUseRange;
    }
}