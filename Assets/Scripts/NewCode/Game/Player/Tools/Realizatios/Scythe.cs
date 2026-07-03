using Garden;
using UnityEngine;

public class Scythe : ITool
{
    public ToolType Type => ToolType.Scythe;
    
    private readonly Player _player;
    private Vector2Int _targetPosition;
    private int _hpCost;
    
    public Scythe(int hpCost)
    {
        _hpCost = hpCost;
    }
        
    public void Activate()
    {
        
    }

    public void Process(IClickSignal signal)
    {
        switch (signal)
        {
            case FieldClickSignal fieldSignal:
                _targetPosition = fieldSignal.Position;
                SignalBus<ChangeHpSignal>.Fire(new ChangeHpSignal(-_hpCost));
                break;
            case EntityClickSignal entitySignal:
                if (entitySignal.Entity.EntityData.Position != _targetPosition)
                {
                    return;
                }
                entitySignal.Entity.EntityData.ForceUseCommands(new MowCommand(entitySignal.Entity.EntityData));
                break;
        }
        
    }
}