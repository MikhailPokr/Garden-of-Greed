using System;
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
        
        private SpeedController _speedController;
        private Player _player;
        
        public event Action<ToolType> ToolChange;
        public event Action<bool[]> ToolLocked;

        public ToolManager(ISpatialMap spatialMap, SpeedController speedController, Player player, List<ITool> toolList)
        {
            _spatialMap = spatialMap;
            _toolList = new Dictionary<ToolType, ITool>();
            _speedController = speedController;
            _player = player;
            
            foreach (var tool in toolList)
            {
                _toolList[tool.Type] = tool;
            }
            
            SignalBus<EntityClickSignal>.OnEvent += OnEntityClick;
            SignalBus<FieldClickSignal>.OnEvent += OnFieldInteract;
            SignalBus<NumPressedSignal>.OnEvent += OnShortcut;
            _speedController.SpeedChange += CalculateLocks;
            _player.FireChanged +=  (_) => CalculateLocks();
            _player.HpChanged +=  (_) => CalculateLocks();
            _player.MoneyChanged +=  (_) => CalculateLocks();
            
            SwitchTool(ToolType.Arm);
        }

        private void OnShortcut(NumPressedSignal signal)
        {
            if (signal.Num is > 0 and < 8 && !_toolList[(ToolType)signal.Num].Locked)
                SwitchTool((ToolType)signal.Num);
        }

        public void SwitchTool(ToolType toolType)
        {
            CurrentTool = toolType;
            if (CurrentTool != ToolType.None)
                _toolList[CurrentTool].Activate();
            ToolChange?.Invoke(toolType);
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
            if (CurrentTool == ToolType.None || _toolList[CurrentTool].Locked)
                return;
            
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

        private void CalculateLocks()
        {
            bool[] locks = new bool[_toolList.Count + 1];
            
            if (_speedController.CurrentSpeed == 0)
                for (var i = 0; i < locks.Length; i++)
                {
                    locks[i] = true;
                }

            if (_player.Money == 0)
            {
                locks[(int)ToolType.TreeShop] = true;
            }

            if (_player.Hp < 1)
            {
                locks[(int)ToolType.Scythe] = true;
                locks[(int)ToolType.Axe] = true;
            }
            
            if (_player.FireData.Count == 0)
            {
                locks[(int)ToolType.Sale] = true;
                locks[(int)ToolType.TreeShop] = true;
                locks[(int)ToolType.Torch] = true;
            }

            foreach (var item in _toolList)
            {
                item.Value.Lock(locks[(int)item.Key]);
            }

            if (CurrentTool != ToolType.None && _toolList[CurrentTool].Locked)
                SwitchTool(ToolType.None);
            
            ToolLocked?.Invoke(locks);
        }
    }
}