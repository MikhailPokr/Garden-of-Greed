using System;
using UnityEngine;

namespace Garden
{
    public class ToolSelectorsController : MonoBehaviour
    {
        [SerializeField] private Color _colorBackNormal;
        [SerializeField] private float _tValue;
        [SerializeField] private ToolSelectionUI[]  _toolSelectionUIs;
        [SerializeField] private Color[] _palette;
        
        private ToolManager _toolManager;
        
        public void Init(ToolManager manager)
        {
            if (_toolSelectionUIs.Length != _palette.Length)
                throw new Exception("the palette colors do not match the tools");
            _toolManager = manager;
            for (var i = 0; i < _toolSelectionUIs.Length; i++)
            {
                var toolSelectionUI = _toolSelectionUIs[i];
                toolSelectionUI.Init(_palette[i], _colorBackNormal, _tValue);
                toolSelectionUI.OnClick += OnClick;
            }

            OnClick(manager.CurrentTool);
            manager.ToolChange += OnToolChange;
            manager.ToolLocked += OnToolLocked;
        }

        private void OnToolLocked(bool[] locks)
        {
            for (int i = 1; i < locks.Length; i++)
            {
                _toolSelectionUIs[i - 1].Lock(locks[i]);
            }
        }

        private void OnToolChange(ToolType type)
        {
            foreach (var toolSelectionUI in _toolSelectionUIs)
            {
                toolSelectionUI.Activate(toolSelectionUI.ToolType == _toolManager.CurrentTool);
            }
        }

        private void OnClick(ToolType type)
        {
            _toolManager.SwitchTool(type);
        }
    }
}