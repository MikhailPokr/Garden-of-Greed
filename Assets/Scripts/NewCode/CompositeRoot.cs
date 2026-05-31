using System;
using UnityEngine;

namespace Garden
{
    public class CompositeRoot : MonoBehaviour
    {
        [SerializeField] private GeneralPalette _generalPalette;
        [SerializeField] private TreePalette _treePalette;
        [SerializeField] private TreeView _treeView;
        [SerializeField] private Field field;
        [Header("Options")]
        [SerializeField] private PlayerStartOptions _startOptions;
        [SerializeField] private TreeGenerationOptions _treeGenerationOptions;
        [SerializeField] private FruitGenerationOptions _fruitGenerationOptions;
        [SerializeField] private BerryGenerationOptions _berryGenerationOptions;
        [SerializeField] private GrassGenerationOptions _grassGenerationOption;
        
        private Player _player;
        private FieldManager _fieldManager;
        private OperationManager _operationManager;
        
        private void Awake()
        {
            _player = new Player(_startOptions);
            _operationManager = new OperationManager();
            _fieldManager = new FieldManager(
                _treeView,
                field,
                _generalPalette,
                _treePalette,
                _player,
                _operationManager,
                _treeGenerationOptions,
                _fruitGenerationOptions,
                _berryGenerationOptions,
                _grassGenerationOption);
            
            _fieldManager.CreateField();
        }

        private void Update()
        {
            float time = Time.deltaTime;
            _player.Update(time);
            _fieldManager.Update(time);
            
        }
    }
}
