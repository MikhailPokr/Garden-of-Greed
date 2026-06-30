using Garden;
using UnityEngine;

public class Scythe : ITool
{
    public ToolType Type => ToolType.Scythe;
    
    private Vector2Int _targetPosition;
        
    public void Activate()
    {
        
    }

    public void Process(IClickSignal signal)
    {
        switch (signal)
        {
            case FieldClickSignal fieldSignal:
                _targetPosition = fieldSignal.Position;
                break;
            case EntityClickSignal entitySignal:
                if (entitySignal.Entity.EntityData.Position != _targetPosition)
                {
                    Debug.Log("Poco raro");
                    return;
                }
                entitySignal.Entity.EntityData.ForceUseCommands(new MowCommand(entitySignal.Entity.EntityData));
                break;
        }
        
    }
}