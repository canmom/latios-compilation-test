using Ecs.Components;
using Input;
using Latios.Transforms;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using LocalTransform = Latios.Transforms.LocalTransform;

namespace Ecs.Systems.Player
{
    public partial struct PlayerMovementSystem : ISystem
    {
        private TransformAspect.Lookup _transformLookup;

        public void OnCreate(ref SystemState state)
        {
            _transformLookup = new TransformAspect.Lookup(ref state);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.CompleteDependency();
            var deltaTime = SystemAPI.Time.DeltaTime;
            _transformLookup.Update(ref state);

            new HorizontalMovementJob() {
                deltaTime = deltaTime,
                transformLookup = _transformLookup,
            }.Run();

            new RotationJob()
            {
                transformLookup = _transformLookup
            }.Run();
            
            new VerticalMovementJob() {}.Run();
            new ZoomJob() {}.Run();
        }

  
        [BurstCompile]
        private partial struct HorizontalMovementJob : IJobEntity
        {
            public float deltaTime;
            public TransformAspect.Lookup transformLookup;

            public void Execute(Entity entity, in TrackingSpace trackingSpace, in MovementInput movementInput)
            {
                var transform = transformLookup[entity];
                var headTransform = transformLookup[trackingSpace.headEntity];
                var speed = movementInput.speed;
                var nextPosition = transform.worldTransform.position;

                if (math.any(math.abs(movementInput.movement) > 0.5f))
                {
                    var delta = headTransform.forwardDirection * movementInput.movement.y +
                                headTransform.rightDirection * movementInput.movement.x;
                    delta.y = 0;
                    delta = math.normalize(delta) * deltaTime;

                    nextPosition += delta * speed * transform.worldScale;
                }
                
                if (math.all(math.isfinite(nextPosition)))
                {
                    transform.worldPosition = nextPosition;
                }
            }
        }

        [BurstCompile]
        private partial struct RotationJob : IJobEntity
        {
            public TransformAspect.Lookup transformLookup;

            public void Execute(Entity entity, in TrackingSpace trackingSpace, in RotationInput rotationInput)
            {
                var transform = transformLookup[entity];
                var headTransform = transformLookup[trackingSpace.headEntity];
                var nextPosition = transform.worldTransform.position;
                var nextRotation = transform.worldTransform.rotation;


                var rotationAmount = 0.0f;
                if (rotationInput.rotateLeft)
                {
                    rotationAmount = math.PI * -0.25f;
                }

                if (rotationInput.rotateRight)
                {
                    rotationAmount = math.PI * 0.25f;
                }

                if (math.abs(rotationAmount) > 0.0f)
                {
                    var rotationQuaternion = quaternion.AxisAngle(math.up(), rotationAmount);
                    Utilities.RotateAroundPoint(ref nextRotation, ref nextPosition, headTransform.worldPosition,
                        rotationQuaternion);
                }


                transform.worldRotation = nextRotation;
            }
        }

        [BurstCompile]
        [WithAll(typeof(TrackingSpace))]
        private partial struct VerticalMovementJob : IJobEntity
        {
            public void Execute(TransformAspect transform, in VerticalMovementInput verticalMovementInput)
            {
                var nextPosition = transform.worldTransform.position;
                
                if (verticalMovementInput.moveUp)
                {
                    nextPosition.y += verticalMovementInput.heightChange * transform.worldScale;
                }

                if (verticalMovementInput.moveDown)
                {
                    nextPosition.y -= verticalMovementInput.heightChange * transform.worldScale;
                }

                if (math.all(math.isfinite(nextPosition)))
                {
                    transform.worldPosition = nextPosition;
                }
            }
        }

        [BurstCompile]
        private partial struct ZoomJob : IJobEntity
        {
            public void Execute(TransformAspect transform, in ZoomInput zoomInput)
            {
                transform.worldScale = zoomInput.zoomFactor;
            }
        }

    }
}

