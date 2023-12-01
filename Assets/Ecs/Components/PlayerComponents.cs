using Unity.Entities;
using Unity.Mathematics;

namespace Ecs.Components
{
    public struct PlayerHead: IComponentData {
    
    }

    public struct PlayerLeftHand : IComponentData
    {
        
    }

    public struct PlayerRightHand : IComponentData
    {
        
    }

    public struct TrackingSpace : IComponentData
    {
        public Entity headEntity;
        public Entity leftHandEntity;
        public Entity rightHandEntity;
        public float2 movement;
    }

    public struct MovementSource : IComponentData { }

    public struct RotationSource : IComponentData { }

    public struct RotationInput : IComponentData
    {
        public bool rotateLeft;
        public bool rotateRight;
        public bool didReset;

    };

    public struct VerticalMovementInput : IComponentData
    {
        public bool moveUp;
        public bool moveDown;
        public float heightChange;
    }

    public struct MovementInput : IComponentData
    {
        public float speed;
        public float2 movement;
    }

    public struct ZoomInput : IComponentData
    {
        public float speed;
        public float minZoom;
        public float maxZoom;
        public float zoomFactor;
    }

}