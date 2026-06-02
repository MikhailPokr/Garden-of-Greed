namespace Garden
{
    public struct VisualContext
    {
        public GeneralPalette GeneralPalette;
        public SpriteOrderOptions SpriteOrder;
        public Field Field;
        public IPalette SpecialPalette;

        public VisualContext(GeneralPalette generalPalette, SpriteOrderOptions spriteOrder, Field field)
        {
            GeneralPalette = generalPalette;
            SpriteOrder = spriteOrder;
            Field = field;
            SpecialPalette = null;
        }
        public VisualContext(GameConfig config, Field field)
        {
            GeneralPalette = config.GeneralPalette;
            SpriteOrder = config.SpriteOrderOptions;
            Field = field;
            SpecialPalette = null;
        }
    }
}