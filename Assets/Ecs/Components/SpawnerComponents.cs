using Unity.Entities;

namespace Ecs.Components
{
    public struct GridSpawner: IComponentData
    {
        public Entity prefab;
        public float xDistance;
        public float yDistance;
        public int rowNumber;
        public int columnNumber;
        public int currentIndex;
    }

    public struct SpawnedEntity : IBufferElementData
    {
        public int row;
        public int column;
        public Entity entity;
    }
}