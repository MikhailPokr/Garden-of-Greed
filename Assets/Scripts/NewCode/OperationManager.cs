using UnityEngine.EventSystems;

namespace Garden
{
    public class OperationManager
    {
        public void RegisterEntity(EntityView view)
        {
            view.ClickAction += OnAction;
        }

        private void OnAction(ClickData data)
        {
            switch (data.InteractionType)
            {
                
            }
        }
    }
}