using Ecs.Components;
using Unity.Entities;
using UnityEngine;

namespace Ecs.Authoring
{
    [DisallowMultipleComponent]
    public class GidSpawnerAuthoring : MonoBehaviour
    {
        public GameObject prefab;
        public float xDistance = 1.0f;
        public float yDistance = 1.0f;
        public int rowNumber = 10;
        public int columnNumber = 10;
        internal class GidSpawnerAuthoringBaker : Baker<GidSpawnerAuthoring>
        {
            public override void Bake(GidSpawnerAuthoring authoring)
            {
                var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                AddComponent(entity, new GridSpawner()
                {
                    prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic),
                    columnNumber = authoring.columnNumber,
                    rowNumber = authoring.rowNumber,
                    xDistance = authoring.xDistance,
                    yDistance = authoring.yDistance,
                });
                AddBuffer<SpawnedEntity>(entity);
            }
        }
    }
}

