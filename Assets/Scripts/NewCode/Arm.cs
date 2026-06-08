using UnityEngine;

namespace Garden
{
    public class Arm
    {
        private Field _field;
        private EntityCreationManager _entityCreationManager;
        private RectInt _bounds;

        private bool _active;
        private EntityCreationRequestSignal _inArm;
        public Arm(Field field, EntityCreationManager entityCreationManager, RectInt bounds)
        {
            _field = field;
            _entityCreationManager = entityCreationManager;
            _bounds = bounds;
            Debug.Log(bounds);
            
            _field.FieldInteract += OnFieldInteract;
            SignalBus<SetInArmSignal>.OnEvent += SetInArm;
        }

        public void SetInArm(SetInArmSignal signal)
        {
            _active = true;
            _inArm = signal.Request;
        }

        private void OnFieldInteract(InteractionType interactionType, Vector2Int position)
        {
            if (!_active || interactionType != InteractionType.Click || !_entityCreationManager.CheckPlace<TreeView>(position))
                return;
            if (!_bounds.Contains(position))
                return;

            _inArm.Position = position;
            SignalBus<EntityCreationRequestSignal>.Fire(_inArm);
            
            _active = false;
        }
    }
}