using UnityEngine;

namespace Garden
{
    public class BurnSignal : ISignal
    {
        public TreeData TreeData { get; }

        public BurnSignal(TreeData treeData)
        {
            TreeData = treeData;
        }
    }
}