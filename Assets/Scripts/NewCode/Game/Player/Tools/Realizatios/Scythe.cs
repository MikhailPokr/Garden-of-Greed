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

    public void Process(InteractionData data)
    {
        if (data.EntityTarget)
        {
            if (data.EntityView.EntityData.Position != _targetPosition)
            {
                return;
            }
            data.EntityView.EntityData.ForceUseCommands(new MowCommand(data.EntityView.EntityData));
        }
        else
        {
            _targetPosition = data.Position;
            SignalBus<ChangeHpSignal>.Fire(new ChangeHpSignal(-_hpCost));
        }
    }
}