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
        private ToolType _currentTool;

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
        
        public void SwithTool(ToolType toolType)
        {
            _currentTool = toolType;
            _toolList[_currentTool].Activate();
        }

        private void OnFieldInteract(FieldClickSignal signal)
        {
            switch (_currentTool)
            {
                case ToolType.TreeShop:
                {
                    if (signal.InteractionType == InteractionType.Click &&
                        _spatialMap.IsTileFreeAndValid(signal.Position))
                    {
                        _toolList[_currentTool].Process(signal);
                    }
                    break;
                }
            }
        }

        private void OnEntityClick(EntityClickSignal signal)
        {
            switch (_currentTool)
            {
                case ToolType.Sell:
                {
                    if (signal.InteractionType == InteractionType.Click &&
                        signal.Entity.EntityData.EntityType == EntityType.Fruit)
                    {
                        _toolList[_currentTool].Process(signal);
                    }
                    break;
                }
            }
        }
    }
}