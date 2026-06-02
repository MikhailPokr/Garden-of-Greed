using UnityEngine;

namespace Garden
{
    public interface IFactory
    {
        IEntityData Create();
        IEntityData Create(int seed);
    }
}