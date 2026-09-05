using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "BerryGenerationOptions", menuName = "Garden/Options/Entities/BerryGenerationOptions")]
    public class BerryGenerationOptions : BaseGenerationOptions
    {
        [SerializeField, Range(0,1)] private float _berryChance;
        [SerializeField] private Vector2Int _regenerationValueRange;
        [SerializeField] private Vector2Int _costRange;
        
        public float GetBerryChance() => _berryChance;
        public Vector2Int GetCostRange() => _costRange;
        public Vector2Int GetRegenerationValueRange() => _regenerationValueRange;
    }
}