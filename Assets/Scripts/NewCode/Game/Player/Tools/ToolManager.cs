using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Garden
{
    public class ToolManager
    {
        private readonly ISpatialMap _spatialMap;
        private readonly Dictionary<ToolType, ITool> _toolList;
        public ToolType CurrentTool {get; private set;}

        public ToolManager(ISpatialMap spatialMap, List<ITool> toolList)
        {
            _spatialMap = spatialMap;
            _toolList = new Dictionary<ToolType, ITool>();
            foreach (var tool in toolList)
            {
                _toolList[tool.Type] = tool;
            }
            
            SignalBus<EntityClickSignal>.OnEvent += OnEntityClick;
            SignalBus<FieldClickSignal>.OnEvent += OnFieldInteract;
        }
        
        public void SwitchTool(ToolType toolType)
        {
            CurrentTool = toolType;
            _toolList[CurrentTool].Activate();
        }

        private void OnFieldInteract(FieldClickSignal signal)
        {
            var data = new InteractionData(CurrentTool, signal.InteractionType, signal.Position);
            switch (CurrentTool)
            {
                case ToolType.TreeShop:
                case ToolType.Arm:
                {
                    if (signal.InteractionType == InteractionType.Click &&
                        _spatialMap.IsTileFreeAndValid(signal.Position))
                    {
                        _toolList[CurrentTool].Process(data);
                    }
                    break;
                }
                case ToolType.Scythe:
                    if (signal.InteractionType == InteractionType.Click)
                    {
                        _toolList[CurrentTool].Process(data);
                    }
                    break;
            }
        }

        private void OnEntityClick(EntityClickSignal signal)
        {
            var data = new InteractionData(CurrentTool, signal.InteractionType, signal.Entity);
            switch (CurrentTool)
            {
                case ToolType.Arm:
                    if (signal.FieldSource)
                        break;
                    if (signal.Entity.EntityType == EntityType.Fruit)
                        if (signal.InteractionType == InteractionType.Click)
                            _toolList[CurrentTool].Process(data);
                    break;
                case ToolType.Mouth:
                    if (signal.FieldSource)
                        break;
                    if (signal.Entity.EntityType is EntityType.Fruit or EntityType.Berry)
                        if (signal.InteractionType == InteractionType.Click)
                            _toolList[CurrentTool].Process(data);
                    break;
                case ToolType.Torch:
                case ToolType.Axe:
                    if (signal.Entity.EntityType != EntityType.Tree)
                        break;
                    goto case ToolType.Scythe;
                case ToolType.Scythe:
                    if (signal.InteractionType == InteractionType.Click && signal.FieldSource)
                        _toolList[CurrentTool].Process(data);
                    break;
                case ToolType.Sale:
                    if (signal.Entity.EntityType is EntityType.Fruit or EntityType.Berry && signal.FieldSource)
                        return;
                    if (signal.InteractionType == InteractionType.Click)
                        _toolList[CurrentTool].Process(data);
                    break;
            }
        }
    }
}