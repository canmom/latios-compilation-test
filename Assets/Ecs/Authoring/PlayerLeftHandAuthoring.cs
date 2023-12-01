using Ecs.Components;
using Unity.Entities;
using UnityEngine;

namespace Ecs.Authoring
{
    [DisallowMultipleComponent]
    public class PlayerLeftHandAuthoring : MonoBehaviour
    {
        internal class LeftHandAuthoringBaker : Baker<PlayerLeftHandAuthoring>
        {
            public override void Bake(PlayerLeftHandAuthoring authoring)
            {
                var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                AddComponent<PlayerLeftHand>(entity);
            }
        }
    }
}

