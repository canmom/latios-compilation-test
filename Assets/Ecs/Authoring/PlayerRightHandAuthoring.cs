using Ecs.Components;
using Unity.Entities;
using UnityEngine;

namespace Ecs.Authoring
{
    [DisallowMultipleComponent]
    public class PlayerRightHandAuthoring : MonoBehaviour
    {
        internal class PlayerRightHandBaker : Baker<PlayerRightHandAuthoring>
        {
            public override void Bake(PlayerRightHandAuthoring authoring)
            {
                var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                AddComponent<PlayerRightHand>(entity);

            }
        }
    }
}

