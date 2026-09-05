using System;
using TMPro;
using UnityEngine;

namespace Garden
{
    public class UIHpCounter : MonoBehaviour
    {
        [SerializeField] private UIHeartElement _heartElementPrefab;
        [SerializeField] private Color[] _palette;
        [SerializeField] private int _count;
        [Space]
        [SerializeField] private Sprite _normalLens;
        [SerializeField] private Sprite _brokenLens;
        
        private Player _player;
        
        private UIHeartElement[] _heartElements;

        public void Init(Player player)
        {
            _player = player;

            if (_player.MaxHp % _count != 0)
                throw new Exception("it is impossible to divide evenly");
            
            int elementHp = _player.MaxHp / _count;
            
            if (_palette.Length > elementHp || _palette.Length < 2)
                throw new Exception("palette length is invalid");

            _heartElements = new UIHeartElement[_count];
            for (int i = 0; i < _count; i++)
            {
                UIHeartElement element = Instantiate(_heartElementPrefab, transform);
                element.Init(elementHp, _palette, _normalLens, _brokenLens);
                _heartElements[i] = element;
            }
            
            _player.HpChanged += OnHpChangeSignal;
        }
        
        private void OnHpChangeSignal(int value)
        {
            for (var i = 0; i < _count; i++)
            {
                var element = _heartElements[i];
                value = element.ChangeHp(value);
            }
        }
    }
}