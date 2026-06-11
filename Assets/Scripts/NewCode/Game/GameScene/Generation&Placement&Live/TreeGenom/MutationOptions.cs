using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(fileName = "MutationOptions", menuName = "Garden/Options/MutationOptions")]
    public class MutationOptions : BaseGenerationOptions
    {
        [SerializeField] private Vector2Int _autoBreedMutationPercentRange;
        [SerializeField] private Vector2Int _evilBreedMutationPercentRange;
        public Vector2Int GetAutoBreedMutationPercentRange(TreeType treeType) =>  treeType switch
        {
            _ when (treeType & TreeType.Evil) != 0 => _evilBreedMutationPercentRange,
            _ => _autoBreedMutationPercentRange
        };
    }
}