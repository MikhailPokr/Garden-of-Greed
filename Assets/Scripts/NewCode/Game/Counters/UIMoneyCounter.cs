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
            _moneyText.text = value.ToString();
        }
    }
}