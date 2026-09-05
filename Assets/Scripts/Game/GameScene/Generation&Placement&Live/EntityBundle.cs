using System;
using UnityEngine;

namespace Garden
{
    [Serializable]
    public struct EntityBundle
    {
        public EntityType EntityType;
        public BasePalette Palette;
        public EntityView EntityView;
        public BaseGenerationOptions GenerationOptions;
    }
}