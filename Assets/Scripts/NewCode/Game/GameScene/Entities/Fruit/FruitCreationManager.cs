using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Garden
{
    public class FruitCreationManager
    {
        private void OnFruitRequest(TreeData treeData)
        {
            /*Vector2Int tPos = (Vector2Int)treeData.Position;

            TreeGenomeConfig config = treeData.TreeGenome;
            
            List<FruitData> fruits = (_factories[EntityType.Fruit] as IMutatableFactory)
                .Create(config)
                .OfType<FruitData>()
                .ToList();

            var fruitsView = new List<EntityView>();
            foreach (var fruit in fruits)
            {
                fruitsView.Add(CreateEntity(new EntityCreationRequestSignal(EntityType.Fruit, fruit, tPos)));
            }

            ((TreeView)Entities[(Vector2Int)treeData.Position].Find(x => x.EntityData == treeData))
                .SetFruits(fruitsView);
                */
        }
    }
}