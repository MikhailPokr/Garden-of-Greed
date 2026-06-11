namespace Garden
{
    public struct VisualContext
    {
        public GeneralPalette GeneralPalette;
        public SpriteOrderOptions SpriteOrder;
        public Field Field;
        public ISpatialMap SpatialMap;
        public IPalette SpecialPalette;
        private InputManager _inputManager;
        public bool Color => _inputManager.Color;
        
        public VisualContext(GameConfig config, Field field, ISpatialMap spatialMap, InputManager inputManager)
        {
            GeneralPalette = config.GeneralPalette;
            SpriteOrder = config.SpriteOrderOptions;
            Field = field;
            SpatialMap = spatialMap;
            SpecialPalette = null;
            _inputManager = inputManager;
        }
    }
}