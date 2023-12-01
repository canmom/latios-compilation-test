using Ecs.Components;
using Latios.Transforms;
using Unity.Entities;
using UnityEngine;

namespace Mono
{
    [DisallowMultipleComponent]
    public class TrackingSpaceMono : MonoBehaviour
    {
        private EntityQuery _trackingspaceQuery;

        private void Start()
        {
            var world = World.All[0];
            _trackingspaceQuery = world.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<TrackingSpace>());
        }

        private void Update()
        {
            var world = World.All[0];
            var entity = _trackingspaceQuery.GetSingletonEntity();
            var trackingSpaceTransform = world.EntityManager.GetComponentData<WorldTransform>(entity);

            transform.SetLocalPositionAndRotation(
                trackingSpaceTransform.position,
                trackingSpaceTransform.rotation);
            transform.localScale = trackingSpaceTransform.scale * Vector3.one;

        }
    }
}