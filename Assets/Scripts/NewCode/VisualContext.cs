namespace Garden
{
    public struct VisualContext
    {
        public GeneralPalette GeneralPalette;
        public SpriteOrderOptions SpriteOrder;
        public Field Field;
        public IPalette SpecialPalette;
        private InputManager _inputManager;
        public bool Color => _inputManager.Color;
        
        public VisualContext(GameConfig config, Field field, InputManager inputManager)
        {
            GeneralPalette = config.GeneralPalette;
            SpriteOrder = config.SpriteOrderOptions;
            Field = field;
            SpecialPalette = null;
            _inputManager = inputManager;
        }
    }
}