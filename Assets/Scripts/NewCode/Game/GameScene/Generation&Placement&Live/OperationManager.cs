using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Garden
{
    public class OperationManager
    {
        public void RegisterEntity(EntityView view)
        {
            view.ClickAction += OnAction;
        }

        private void OnAction(EntityView view, InteractionType type)
        {
            if (type == InteractionType.Click && view.EntityData.EntityType == EntityType.Fruit)
            {
                view.EntityData.Destroy();
            }
        }
    }
}