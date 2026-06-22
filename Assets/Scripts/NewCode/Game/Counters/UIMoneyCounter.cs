using System;
using TMPro;
using UnityEngine;

namespace Garden
{
    public class UIMoneyCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyText;
        
        private Player _player;

        public void Init(Player player)
        {
            _player = player;
            _player.OnChangeMoney += OnMoneyChangeSignal;
            OnMoneyChangeSignal();
        }
        
        private void OnMoneyChangeSignal()
        {
            moneyText.text = _player.Money.ToString();
        }
    }
}