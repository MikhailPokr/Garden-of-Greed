using TMPro;
using UnityEngine;

namespace Garden
{
    public class UIHpCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _hpText;
        
        private Player _player;

        public void Init(Player player)
        {
            _player = player;
            _player.HpChanged += OnHpChangeSignal;
        }
        
        private void OnHpChangeSignal(int value)
        {
            _hpText.text = value.ToString();
        }
    }
}