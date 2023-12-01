﻿using Ecs.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Input
{
    public partial class LeftHandInputSystem : SystemBase
    {
        
        private PlayerInput _playerInput;

        protected override void OnCreate()
        {
            _playerInput = new PlayerInput();
            var handEntity = EntityManager.CreateEntity();
            EntityManager.AddComponent<LeftController>(handEntity);
            EntityManager.AddComponent<ControllerInput>(handEntity);
            EntityManager.AddComponent<RotationSource>(handEntity);
            EntityManager.SetName( handEntity, "LeftController");
        }

        protected override void OnStartRunning()
        {
            _playerInput.Enable();
        }

        protected override void OnStopRunning()
        {
            _playerInput.Disable();
        }

        protected override void OnUpdate()
        {
            var input = default(ControllerInput);
            var controller = _playerInput.LeftHandController;
            input.position = controller.Position.ReadValue<Vector3>();
            input.rotation = controller.Rotation.ReadValue<Quaternion>();
            if (math.any(math.isinf(input.position)))
            {
                input.position = float3.zero;
            }

        
            
            input.triggerButton.wasPressedThisFrame = controller.Trigger.WasPressedThisFrame();
            input.triggerButton.wasReleasedThisFrame = controller.Trigger.WasReleasedThisFrame();
            input.triggerButton.isPressed = controller.Trigger.IsPressed();


            input.grabButton.wasPressedThisFrame = controller.Grip.WasPressedThisFrame();
            input.grabButton.wasReleasedThisFrame = controller.Grip.WasReleasedThisFrame();
            input.grabButton.isPressed = controller.Grip.IsPressed();

            input.primaryButton.wasPressedThisFrame = controller.PrimaryButton.WasPressedThisFrame();
            input.primaryButton.wasReleasedThisFrame = controller.PrimaryButton.WasReleasedThisFrame();
            input.primaryButton.isPressed = controller.PrimaryButton.IsPressed();

            input.secondaryButton.wasPressedThisFrame = controller.SecondaryButton.WasPressedThisFrame();
            input.secondaryButton.wasReleasedThisFrame = controller.SecondaryButton.WasReleasedThisFrame();
            input.secondaryButton.isPressed = controller.SecondaryButton.IsPressed();

            input.thumbStick = controller.ThumbStick.ReadValue<Vector2>();
            
            var handEntity = SystemAPI.QueryBuilder()
                                      .WithAll<LeftController>()
                                      .WithAll<ControllerInput>()
                                      .Build()
                                      .GetSingletonEntity();
            EntityManager.SetComponentData(handEntity, input);
        }

    }
}