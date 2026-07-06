using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Garden
{
    public class ArsonManager
    {
        private readonly int _seed;
        private int _seedUsage;
        
        private readonly ISpatialMap _spatialMap;
        private readonly EntityCreationManager _entityCreationManager;
        private readonly float _burnInterval;
        private readonly float _burnChance;

        private List<ArsonElement> _next;

        public ArsonManager(int seed, ISpatialMap spatialMap,EntityCreationManager entityCreationManager, float burnInterval, float burnChance)
        {
            _seed = SeedUtils.GetNewSeed(seed, SeedUserType.ArsonManager);
            
            _spatialMap = spatialMap;
            _entityCreationManager = entityCreationManager;
            _burnInterval = burnInterval;
            _burnChance = burnChance;
            
            _next = new List<ArsonElement>();
            SignalBus<BurnSignal>.OnEvent += OnBurn;
        }
        
        public void Update(float deltaTime)
        { 
            for (var i = 0; i < _next.Count; i++)
            {
                _next[i].Timer += deltaTime;
                if (_next[i].Timer < _burnInterval)
                    continue;
                _next[i].Timer -= _burnInterval;
                var next = _next[i].Next.ToList();
                _next[i].Next.Clear();
                foreach (var tree in next)
                {
                    if (!_entityCreationManager.CreatedEntities.Contains(tree))
                        continue;
                    
                    _next[i].Next.AddRange(GetNext(tree.Position));
                    tree.ForceUseCommands(new BurnCommand(tree));
                }
            }
            _next.RemoveAll(x => x.Next.Count == 0);
        }

        private void OnBurn(BurnSignal signal)
        {
            var trees = GetNext(signal.TreeData.Position);
            _next.Add(new ArsonElement()
            {
                Next = trees,
                Timer = 0f,
            });
        }

        private List<TreeData> GetNext(Vector2Int position)
        {
            var neighbors = _spatialMap.GetNeighbors(position);
            var trees = new List<TreeData>();
            for (var i = 0; i < neighbors.Count; i++)
            {
                var value = SeedUtils.GetRandom(_seed, ParamType.BurnChance + _seedUsage++);
                if (value < _burnChance)
                {
                    var tree = _entityCreationManager.CreatedEntities.Find(x =>
                        x.Position == neighbors[i] && x.EntityType == EntityType.Tree);
                    if (tree != null)
                    {
                        trees.Add(tree as TreeData);
                    }
                }
            }
            return trees;
        }
    }
}