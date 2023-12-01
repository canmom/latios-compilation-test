using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecs
{
    [BurstCompile]
    public static class Utilities
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RotateAroundPoint(ref quaternion rotation, ref float3 position, float3 aroundPoint, quaternion addedRotation)
        {
            float3 localPointToTranslation = math.mul(math.inverse(rotation), position - aroundPoint);
            rotation = math.mul(rotation, addedRotation);
            position = aroundPoint + math.mul(rotation, localPointToTranslation);
        }
        
        [BurstCompile]
        private static IEnumerable<ComponentType> RequiredComponents<T>(this IAspectCreate<T> create, bool isReadonly)
            where T : IAspect
        {
            var list = new UnsafeList<ComponentType>(10, Allocator.Temp);
            create.AddComponentRequirementsTo(ref list);
 
            foreach (var type in list)
            {
                yield return type;
            }
 
            list.Dispose();
        }
 
        [BurstCompile]
        public static bool TryAspect<TAspect>(this EntityManager entityManager, Entity entity, bool isReadonly, out TAspect aspect)
            where TAspect : unmanaged, IAspect, IAspectCreate<TAspect>
        {
            var create = new TAspect();
 
            foreach (var component in create.RequiredComponents(isReadonly))
            {
                if (entityManager.HasComponent(entity, component)) continue;
                aspect = default;
                return false;
            }
 
            aspect = entityManager.GetAspect<TAspect>(entity);
            return true;
        }
    }
}