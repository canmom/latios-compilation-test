using Ecs.Components;
using Input;
using Latios.Transforms;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Ecs.Systems.Spawning
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct GridSpawnerSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GridSpawner>();
            state.RequireForUpdate<SpawnedEntity>();
        }


        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.CompleteDependency();
            var controller = SystemAPI.QueryBuilder()
                                      .WithAll<ControllerInput>()
                                      .WithAll<RightController>()
                                      .Build()
                                      .GetSingleton<ControllerInput>();

            var buffer = SystemAPI.GetSingletonBuffer<SpawnedEntity>();

            if (controller.triggerButton.wasPressedThisFrame)
            {
                var spawner = SystemAPI.GetSingleton<GridSpawner>();
                var spawnerTransform = SystemAPI.GetComponent<WorldTransform>(SystemAPI.GetSingletonEntity<GridSpawner>());
                var position = spawnerTransform.position;
                var row = buffer.Length / spawner.columnNumber;
                var column = buffer.Length % spawner.columnNumber;


                position.x += column * spawner.xDistance;
                position.y += row * spawner.yDistance;

                var entity = state.EntityManager.Instantiate(spawner.prefab);
                ref var transform = ref SystemAPI.GetComponentRW<WorldTransform>(entity).ValueRW;
                transform.worldTransform.position = position;
                buffer.Add(new SpawnedEntity()
                {
                    column = column,
                    row = row,
                    entity = entity,
                });
            }
            else if (controller.grabButton.wasPressedThisFrame && buffer.Length > 0)
            {
                var element = buffer[^1];
                buffer.RemoveAt(buffer.Length - 1);
                state.EntityManager.DestroyEntity(element.entity);
            }
        }
    }
}