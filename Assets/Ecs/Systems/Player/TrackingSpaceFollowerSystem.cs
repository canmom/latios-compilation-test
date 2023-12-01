// using Ecs.Components;
// using Latios.Transforms;
// using Mono;
// using Unity.Entities;
// using UnityEngine;
//
// namespace Ecs.Systems.Player
// {
//     [UpdateInGroup(typeof(SimulationSystemGroup))]
//     public partial class TrackingSpaceFollowerSystem : SystemBase
//     {
//         private TrackingSpaceMono _localTrackingSpace;
//         
//         protected override void OnStartRunning()
//         {
//             _localTrackingSpace = GameObject.FindObjectOfType<TrackingSpaceMono>();
//         }
//
//         protected override void OnUpdate()
//         {
//             if (_localTrackingSpace == null)
//             {
//                 _localTrackingSpace = GameObject.FindObjectOfType<TrackingSpaceMono>();
//                 return;
//             }
//             
//             Entities
//                .WithAll<TrackingSpace>()
//                .WithoutBurst()
//                .ForEach((Entity entity, TransformAspect transform) =>
//                 {
//                     _localTrackingSpace.transform.SetPositionAndRotation(transform.worldPosition,
//                         transform.worldRotation);
//                 }).Run();
//         }
//     }
// }