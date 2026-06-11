using UnityEngine;

namespace Garden
{
    [System.Serializable]
    public class FieldOptions
    {
        public float CellWidth;
        public float RowHeight;
        public Vector3 Center;
        public RectInt Bounds;
    }
}