using Ecs.Components;
using Unity.Entities;
using UnityEngine;

namespace Ecs.Authoring
{
    [DisallowMultipleComponent]
    public class PlayerHeadAuthoring : MonoBehaviour
    {
        internal class PlayerHeadAuthoringBaker : Baker<PlayerHeadAuthoring>
        {
            public override void Bake(PlayerHeadAuthoring authoring)
            {
                var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                AddComponent<PlayerHead>(entity);
            }
        }
    }
}

