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
            switch (CurrentTool)
            {
                case ToolType.TreeShop:
                case ToolType.Arm:
                {
                    if (signal.InteractionType == InteractionType.Click &&
                        _spatialMap.IsTileFreeAndValid(signal.Position))
                    {
                        _toolList[CurrentTool].Process(signal);
                    }
                    break;
                }
                case ToolType.Scythe:
                    if (signal.InteractionType == InteractionType.Click)
                    {
                        _toolList[CurrentTool].Process(signal);
                    }
                    break;
            }
        }

        private void OnEntityClick(EntityClickSignal signal)
        {
            switch (CurrentTool)
            {
                case ToolType.Arm:
                    if (signal.Entity.EntityType == EntityType.Fruit)
                        goto case ToolType.Sale;
                    break;
                case ToolType.Scythe:
                case ToolType.Sale:
                {
                    if (signal.InteractionType == InteractionType.Click)
                        _toolList[CurrentTool].Process(signal);
                    break;
                }
                
                    
            }
        }
    }
}