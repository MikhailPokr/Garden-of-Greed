using UnityEngine;
using UnityEngine.EventSystems;

namespace Garden
{
    public class ToolSelectionUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private ToolType _toolType;
        
        private ToolManager _toolManager;

        public void Init(ToolManager toolManager)
        {
            _toolManager = toolManager;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _toolManager.SwithTool(_toolType);
        }
    }
}
