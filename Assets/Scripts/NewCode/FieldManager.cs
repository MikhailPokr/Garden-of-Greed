using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Garden
{
    public class FieldManager
    {
        public event Action<float> ColorChanged;
        
        public readonly int _seed;
        private readonly Field _fieldPrefab;
        public readonly GeneralPalette GeneralPalette;
        public readonly Player Player;
        
        private Field _field;
        
        public FieldManager(
            int seed,
            Field field,
            GeneralPalette generalPalette,
            Player player)
        {
            _seed = SeedUtils.GetNewSeed(seed, SeedUserType.Field);
            _fieldPrefab = field;
            GeneralPalette = generalPalette;
            Player = player;
        }

        public Field CreateField()
        {
            _field = Object.Instantiate(_fieldPrefab);
            _field.Init(this);
            
            ColorChanged?.Invoke(0);
            return _field;
        }


        
    }
}