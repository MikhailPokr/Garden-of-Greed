using System;
using TMPro;
using UnityEngine;

namespace Garden
{
    public class UIMoneyCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _moneyText;
        
        private Player _player;

        public void Init(Player player)
        {
            _player = player;
            _player.MoneyChanged += OnMoneyChangeSignal;
        }
        
        private void OnMoneyChangeSignal(int value)
        {
            var str = value.ToString("D12");
            var text = "";
            for (int i = 0; i < 3; i++)
            {
                text += str.Substring(i * 3, 3);
                text += ".";
            }
            text += str.Substring(9, 3);
            _moneyText.text = text;
        }
    }
}