using UnityEngine;

namespace Garden
{
    [CreateAssetMenu(menuName = "Garden/Options/PlayerStartOptions", fileName = "PlayerStartOptions")]
    public class PlayerStartOptions : ScriptableObject
    {
        [field: SerializeField] public int StartMoney { get; set; }
        [field: SerializeField] public int MaxHp { get; set; }
        [field: SerializeField] public int MaxFire { get; set; }
        [field: SerializeField] public int StartFireForce { get; set; }
    }
}