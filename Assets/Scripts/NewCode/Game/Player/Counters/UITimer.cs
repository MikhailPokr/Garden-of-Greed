using System;
using TMPro;
using UnityEngine;

namespace Garden
{
    public class UITimer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timeText;

        private Player _player;
        
        public void Init(Player player)
        {
            _player = player;
            UpdateView();
        }
        
        public void UpdateView()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds((long)_player.Time);
            _timeText.text = timeSpan.ToString(@"hh\:mm\:ss");
        }
    }
}