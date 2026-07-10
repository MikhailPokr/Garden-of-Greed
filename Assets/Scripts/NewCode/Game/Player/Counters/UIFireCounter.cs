using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Garden
{
    public class UIFireCounter : MonoBehaviour
    {
        [SerializeField] private UIFireElement _fireElementPrefab;
        [Space]
        [SerializeField] private Color[] _paletteNormal;
        [SerializeField] private Color[] _paletteEvil;
        
        private Dictionary<FireData, UIFireElement> _fireElements;
        
        private Player _player;

        public void Init(Player player)
        {
            _player = player;
            _player.FireChanged += OnFireChange;
            _fireElements = new Dictionary<FireData, UIFireElement>();
        }

        public void UpdateLogic()
        {
            foreach (var fireData in _fireElements)
            {
                fireData.Value.UpdateView();
            }
        }

        private void OnFireChange(List<FireData> list)
        {
            var ext = _fireElements.Keys.Where(x => !list.Contains(x)).ToList();
            foreach (var data in ext)
            {
                _fireElements[data].Destroy();
                _fireElements.Remove(data);
            }

            var newData = list.Where(x => !_fireElements.ContainsKey(x)).ToList();
            foreach (var data in newData)
            {
                var newFire = Instantiate(_fireElementPrefab, transform);
                newFire.transform.SetSiblingIndex(transform.childCount - 2);
                    
                _fireElements.Add(data, newFire);
                _fireElements[data].Init(data, data.IsEvilFire ? _paletteEvil : _paletteNormal);
            }
        }
    }
}