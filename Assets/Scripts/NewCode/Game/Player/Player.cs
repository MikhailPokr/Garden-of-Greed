using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Garden
{
    public class Player
    {
        public int Money { get; private set; }
        public int Hp { get; private set; }
        public List<FireData> FireData { get; private set; }
        public float Time { get; private set; }
        
        private readonly PlayerStartOptions _startOptions;
        private readonly GameConfig _gameConfig;
        private ITool _currentTool;

        public event Action<int> HpChanged;
        public event Action<int> MoneyChanged;
        public event Action<int> FireChanged;


        public Player(PlayerStartOptions startOptions, GameConfig config)
        {
            _startOptions = startOptions;
            _gameConfig = config;
            FireData = new List<FireData>();
            SignalBus<ChangeMoneySignal>.OnEvent += OnChangeMoney;
            SignalBus<ChangeHpSignal>.OnEvent += OnChangeHp;
            SignalBus<AddFireSignal>.OnEvent += OnAddFire;
        }

        private void OnAddFire(AddFireSignal signal)
        {
            var fire = new FireData(signal.FuelForce * _gameConfig.FireOptions.FireTimePerPoint, signal.IsEvil);
            FireData.Add(fire);
            fire.TimeIsOver += OnFireIsOver;
            FireChanged?.Invoke(FireData.Count);
        }

        private void OnFireIsOver(FireData obj)
        {
            obj.TimeIsOver -= OnFireIsOver;
            FireData.Remove(obj);
            FireChanged?.Invoke(FireData.Count);
        }

        public void Start()
        {
            OnChangeMoney(new  ChangeMoneySignal(_startOptions.StartMoney));
            OnChangeHp(new ChangeHpSignal(_startOptions.MaxHp));
            OnAddFire(new AddFireSignal(_startOptions.StartFireForce));
        }

        public void Update(float deltaTime)
        {
            Time += deltaTime;

            var mult = 1f;
            for (int i = 0; i < FireData.Count; i++)
            {
                if (FireData[i].IsEvilFire)
                    mult *= _gameConfig.FireOptions.EvilFireTimeMultiplier;
                else
                    mult *= _gameConfig.FireOptions.NormalFireMultiplier;
            }
            deltaTime *= mult;
            for (var i = 0; i < FireData.Count; i++)
            {
                FireData[i].Update(deltaTime);
            }
        }

        public void OnChangeMoney(ChangeMoneySignal signal)
        {
            Money += signal.Delta;
            MoneyChanged?.Invoke(Money);
        }
        private void OnChangeHp(ChangeHpSignal signal)
        {
            Hp += signal.Delta;
            
            Hp = Mathf.Clamp(Hp, 0, _startOptions.MaxHp);
            if (Hp == 0)
            {
                //
            }
            HpChanged?.Invoke(Hp);
        }
    }
}