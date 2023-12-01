using Ecs.Components;
using Unity.Entities;
using UnityEngine;

namespace Ecs.Authoring
{
    [DisallowMultipleComponent]
    public class TrackingSpaceAuthroing : MonoBehaviour
    {

        public float movementSpeed = 1.0f;
        public float heightChange = 0.1f;
        public float zoomSpeed = 0.1f;
        public float minZoom = 0.1f;
        public float maxZoom = 30.0f;
        public PlayerHeadAuthoring playerHead;
        public PlayerLeftHandAuthoring playerLeftHand;
        public PlayerRightHandAuthoring playerRightHand;
        internal class TrackingSpaceAuthroingBaker : Baker<TrackingSpaceAuthroing>
        {
            public override void Bake(TrackingSpaceAuthroing authoring)
            {
                var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                AddComponent(entity, new TrackingSpace()
                {
                    headEntity = GetEntity(authoring.playerHead, TransformUsageFlags.Dynamic),
                    leftHandEntity = GetEntity(authoring.playerLeftHand, TransformUsageFlags.Dynamic),
                    rightHandEntity = GetEntity(authoring.playerRightHand, TransformUsageFlags.Dynamic),

                });
                AddComponent(entity, new MovementInput()
                {
                    speed = authoring.movementSpeed,
                });
                AddComponent<RotationInput>(entity);
                AddComponent(entity, new VerticalMovementInput()
                {
                    heightChange = authoring.heightChange,
                });
                
                AddComponent(entity, new ZoomInput()
                {
                    zoomFactor = 1.0f,
                    speed = authoring.zoomSpeed,
                    minZoom = authoring.minZoom,
                    maxZoom = authoring.maxZoom,
                });
            }
        }
    }
}

