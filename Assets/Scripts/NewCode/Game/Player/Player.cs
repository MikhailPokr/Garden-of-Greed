using System;
using UnityEngine;

namespace Garden
{
    public class Player
    {
        public int Money { get; private set; }
        public int Hp { get; private set; }
        public float Time { get; private set; }
        
        private readonly PlayerStartOptions _startOptions;
        private ITool _currentTool;

        public event Action<int> HpChanged;
        public event Action<int> MoneyChanged;


        public Player(PlayerStartOptions startOptions)
        {
            _startOptions = startOptions;
            SignalBus<ChangeMoneySignal>.OnEvent += OnChangeMoney;
            SignalBus<ChangeHpSignal>.OnEvent += OnChangeHp;
        }
        public void Start()
        {
            OnChangeMoney(new  ChangeMoneySignal(_startOptions.StartMoney));
            OnChangeHp(new ChangeHpSignal(_startOptions.MaxHp));
        }

        public void Update(float deltaTime)
        {
            Time += deltaTime;
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