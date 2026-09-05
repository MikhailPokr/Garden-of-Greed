using UnityEngine;

namespace Garden
{
    public class FenceGenerator
    {
        private readonly IGridMath _gridMath;
        private readonly GeneralPalette _generalPalette;

        public FenceGenerator(IGridMath gridMath, GeneralPalette palette)
        {
            _gridMath = gridMath;
            _generalPalette = palette;
        }

        public void GenerateFence()
        {
            var points = _gridMath.GetOutBounds();
            GameObject parent = new GameObject("Fence"); 
            foreach (var point in points)
            {
                var fence = Object.Instantiate(_generalPalette.Fence, parent.transform);
                fence.transform.position = _gridMath.GetPoint(point);
            }
        }
    }
}