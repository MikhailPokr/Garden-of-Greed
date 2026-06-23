using UnityEngine;
using UnityEngine.UI;

namespace Garden
{
    public class ToolSelectorsController : MonoBehaviour
    {
        [SerializeField] private ToolSelectionUI[]  _toolSelectionUIs;
        
        ToolManager _toolManager;
        
        public void Init(ToolManager manager)
        {
            _toolManager = manager;
            foreach (var toolSelectionUI in _toolSelectionUIs)
            {
                toolSelectionUI.OnClick += OnClick;
            }
            OnClick(manager.CurrentTool);
        }

        private void OnClick(ToolType type)
        {
            _toolManager.SwithTool(type);
            
            foreach (var toolSelectionUI in _toolSelectionUIs)
            {
                Color color = toolSelectionUI.Image.color;
                if (toolSelectionUI.ToolType == _toolManager.CurrentTool)
                    color.a = 1;
                else
                    color.a = 0.5f;
                toolSelectionUI.Image.color = color;
            }
        }
    }
}