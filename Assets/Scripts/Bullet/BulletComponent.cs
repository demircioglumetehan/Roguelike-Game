using Unity.Entities;
using Unity.Mathematics;

namespace RogueLikeGame.ECS.BulletSystem
{
    public struct BulletComponent : IComponentData
    {
        public int Damage;
        public float MovementSpeed;
        public float3 movementDirection;
        public bool HasGroundEffect;
        public Entity GroundEffect;
    }

}
