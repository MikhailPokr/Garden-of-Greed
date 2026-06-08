using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public interface IFactory
    {
        IEntityData Create(EntityCreationRequestSignal signal);
        List<IEntityData> Create(EntityView origin);
    }
}