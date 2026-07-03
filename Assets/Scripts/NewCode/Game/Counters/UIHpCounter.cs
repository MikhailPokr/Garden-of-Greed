using TMPro;
using UnityEngine;

namespace Garden
{
    public class UIHpCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyText;
        
        private Player _player;

        public void Init(Player player)
        {
            _player = player;
            _player.HpChanged += OnHpChangeSignal;
        }
        
        private void OnHpChangeSignal(int value)
        {
            moneyText.text = value.ToString();
        }
    }
}