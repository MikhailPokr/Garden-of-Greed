using System;
using System.Collections.Generic;

namespace Garden
{
    public class FruitFactory : IFactory
    {
        private readonly FruitPalette _fruitPalette;
        private readonly FruitGenerationOptions _options;
        public FruitFactory(EntityBundle bundle)
        {
            _fruitPalette = (FruitPalette)bundle.Palette;
            _options = (FruitGenerationOptions)bundle.Options;
        }
        
        public IEntityData Create(EntityCreationRequestSignal signal)
        {
            throw new NotImplementedException();
        }

        public List<IEntityData> Create(EntityView origin)
        {
            throw new NotImplementedException();
        }

        public FruitData Create(TreeData treeData)
        {
            throw new NotImplementedException();
        }


        
    }
}