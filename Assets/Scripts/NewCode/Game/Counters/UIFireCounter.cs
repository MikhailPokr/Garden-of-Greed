using System;
using TMPro;
using UnityEngine;

namespace Garden
{
    public class UIFireCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _fireText;
        
        private Player _player;

        public void Init(Player player)
        {
            _player = player;
            _player.FireChanged += OnFireChangeSignal;
        }
        
        private void OnFireChangeSignal(int value)
        {
            _fireText.text = value.ToString();
        }
    }
}