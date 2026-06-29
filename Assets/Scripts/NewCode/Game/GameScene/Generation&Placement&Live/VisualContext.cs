namespace Garden
{
    public struct VisualContext
    {
        public readonly GeneralPalette GeneralPalette;
        public readonly SpriteOrderOptions SpriteOrder;
        public readonly  Field Field;
        public readonly ISpatialMap SpatialMap;
        
        private readonly InputManager _inputManager;
        
        public IPalette SpecialPalette;
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