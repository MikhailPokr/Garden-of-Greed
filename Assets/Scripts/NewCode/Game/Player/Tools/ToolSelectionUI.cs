using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Garden
{
    public class ToolSelectionUI : MonoBehaviour, IPointerClickHandler
    {
        [field: SerializeField] public ToolType ToolType { get; private set; }
        [field: SerializeField] public Image Image { get; private set; }

        public event Action<ToolType> OnClick;
        
        public void OnPointerClick(PointerEventData eventData) => OnClick?.Invoke(ToolType);
    }
}
