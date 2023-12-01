using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Input
{
    public partial class HeadInputSystem : SystemBase
    {
       

        private PlayerInput _playerInput;

        protected override void OnCreate()
        {
            _playerInput = new PlayerInput();
            var headEntity = EntityManager.CreateEntity();
            EntityManager.AddComponent<HeadsetInput>(headEntity);
            RequireForUpdate<HeadsetInput>();
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
            var input = default(HeadsetInput);
            var headset = _playerInput.Headset;
            input.position = headset.Position.ReadValue<Vector3>();
            input.rotation = headset.Rotation.ReadValue<Quaternion>();
            if (math.any(math.isinf(input.position)))
            {
                input.position = float3.zero;
            }

            var headInputEntity = SystemAPI.GetSingletonEntity<HeadsetInput>();
            EntityManager.SetComponentData(headInputEntity, input);

        }
    }
}