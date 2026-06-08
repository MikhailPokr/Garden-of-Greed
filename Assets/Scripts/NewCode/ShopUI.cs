using UnityEngine;
using UnityEngine.EventSystems;

namespace Garden
{
    public class ShopUI : MonoBehaviour, IPointerClickHandler
    {
        private Shop _shop;

        public void Init(Shop shop)
        {
            _shop = shop;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("OnPointerClick");
            SignalBus<SetInArmSignal>.Fire(new SetInArmSignal(_shop.Get()));
        }
    }
}
