using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Input
{
    [Serializable]
    public struct ButtonPress
    {
        public bool isPressed;
        public bool wasPressedThisFrame;
        public bool wasReleasedThisFrame;
    }
    public struct ControllerInput: IComponentData
    {
        public float3 position;

        public quaternion rotation;

        public ButtonPress grabButton;

        public ButtonPress triggerButton;

        public ButtonPress primaryButton;

        public ButtonPress secondaryButton;

        public float2 thumbStick;
    }

    public struct HeadsetInput : IComponentData
    {
        public float3 position;
        public quaternion rotation;
    }

    public struct RightController : IComponentData { }

    public struct LeftController : IComponentData { }
}