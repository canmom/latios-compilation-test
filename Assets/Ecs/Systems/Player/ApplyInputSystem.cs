using Ecs.Components;
using Input;
using Latios.Transforms;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Ecs.Systems.Player
{
    [UpdateAfter(typeof(LeftHandInputSystem))]
    [UpdateAfter(typeof(RightHandInputSystem))]
    [UpdateBefore(typeof(PlayerMovementSystem))]
    public partial struct ApplyInputSystem : ISystem
    {
        public void OnCreate(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var movementInputQuery
                = SystemAPI.QueryBuilder().WithAll<ControllerInput>().WithAll<MovementSource>().Build();
            var deltaTime = SystemAPI.Time.DeltaTime;
            if (!movementInputQuery.IsEmpty)
            {
                var handInput = movementInputQuery.GetSingleton<ControllerInput>();

                foreach (var nextInput in SystemAPI.Query<RefRW<MovementInput>>().WithAll<TrackingSpace>())
                {
                    nextInput.ValueRW.movement = handInput.thumbStick;
                }

                foreach (var nextInputRef in SystemAPI.Query<RefRW<ZoomInput>>().WithAll<TrackingSpace>())
                {
                    ref var nextInput = ref nextInputRef.ValueRW;
                    if (handInput.secondaryButton.isPressed)
                    {
                        nextInput.zoomFactor += nextInput.speed * deltaTime;
                    }

                    if (handInput.primaryButton.isPressed)
                    {
                        nextInput.zoomFactor -= nextInput.speed * deltaTime;
                    }

                    nextInput.zoomFactor = math.clamp(nextInput.zoomFactor, nextInput.minZoom, nextInput.maxZoom);
                }
            }

            var verticalInputQuery
                = SystemAPI.QueryBuilder().WithAll<ControllerInput>().WithAll<RotationSource>().Build();

            if (!verticalInputQuery.IsEmpty)
            {
                var handInput = verticalInputQuery.GetSingleton<ControllerInput>();

                foreach (var nextInput in SystemAPI.Query<RefRW<VerticalMovementInput>>().WithAll<TrackingSpace>())
                {
                    nextInput.ValueRW.moveUp = handInput.secondaryButton.wasPressedThisFrame;
                    nextInput.ValueRW.moveDown = handInput.primaryButton.wasPressedThisFrame;
                }
            }

            var rotationInputQuery
                = SystemAPI.QueryBuilder().WithAll<ControllerInput>().WithAll<RotationSource>().Build();

            if (!rotationInputQuery.IsEmpty)
            {
                var handInput = rotationInputQuery.GetSingleton<ControllerInput>();

                foreach (var nextInput in SystemAPI.Query<RefRW<RotationInput>>().WithAll<TrackingSpace>())
                {
                    nextInput.ValueRW.rotateLeft = false;
                    nextInput.ValueRW.rotateRight = false;

                    if (!nextInput.ValueRW.didReset && math.abs(handInput.thumbStick.x) < 0.1f)
                    {
                        nextInput.ValueRW.didReset = true;
                    }

                    if (nextInput.ValueRO.didReset && handInput.thumbStick.x < -0.5f)
                    {
                        nextInput.ValueRW.rotateLeft = true;
                        nextInput.ValueRW.didReset = false;
                    }

                    if (nextInput.ValueRO.didReset && handInput.thumbStick.x > 0.5f)
                    {
                        nextInput.ValueRW.rotateRight = true;
                        nextInput.ValueRW.didReset = false;
                    }
                }
            }


            var rightHandInput = SystemAPI.QueryBuilder().WithAll<ControllerInput>().WithAll<RightController>().Build()
                                          .GetSingleton<ControllerInput>();

            foreach (var rightHandTransform in SystemAPI.Query<TransformAspect>().WithAll<PlayerRightHand>())
            {
                rightHandTransform.localPosition = rightHandInput.position;
                rightHandTransform.localRotation = rightHandInput.rotation;
            }

            var leftHandInput = SystemAPI.QueryBuilder().WithAll<ControllerInput>().WithAll<LeftController>().Build()
                                         .GetSingleton<ControllerInput>();

            foreach (var leftHandTransform in SystemAPI.Query<TransformAspect>().WithAll<PlayerLeftHand>())
            {
                leftHandTransform.localPosition = leftHandInput.position;
                leftHandTransform.localRotation = leftHandInput.rotation;
            }
        }
    }
}