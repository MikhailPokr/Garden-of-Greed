using UnityEngine;

namespace Garden
{
    public struct InteractionData
    {
        public ToolType ToolType;
        public InteractionType InteractionType;
        public Vector2Int Position;
        public bool EntityTarget;
        public EntityView EntityView;

        public InteractionData(ToolType toolType, InteractionType interactionType, Vector2Int position)
        {
            EntityTarget = false;
            ToolType = toolType;
            InteractionType = interactionType;
            Position = position;
            EntityView = null;
        }
        public InteractionData(ToolType toolType, InteractionType interactionType, EntityView entityView)
        {
            EntityTarget = true;
            ToolType = toolType;
            InteractionType = interactionType;
            EntityView = entityView;
            Position = EntityView.EntityData.Position;
        }
    }
}