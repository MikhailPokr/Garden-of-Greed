using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Garden
{
    public class FieldManager
    {   
        public readonly int _seed;
        private readonly Field _fieldPrefab;
        public readonly GeneralPalette GeneralPalette;
        public readonly Player Player;
        private readonly RectInt _bounds;
        
        private Field _field;
        
        public FieldManager(
            int seed,
            Field field,
            GeneralPalette generalPalette,
            Player player,
            RectInt bounds)
        {
            _seed = SeedUtils.GetNewSeed(seed, SeedUserType.Field);
            _fieldPrefab = field;
            GeneralPalette = generalPalette;
            Player = player;
            _bounds = bounds;
        }

        public void Update()
        {
            if (_field != null)
                _field.UpdateLogic();
        }

        public Field CreateField()
        {
            _field = Object.Instantiate(_fieldPrefab);
            _field.Init(this, _bounds);
            
            SignalBus<ColorModeChangedSignal>.Fire(new(false));
            return _field;
        }
    }
}