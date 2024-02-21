using Unity.Entities;
using Unity.Mathematics;

namespace RogueLikeGame.ECS.Player
{
    public struct PlayerComponent : IComponentData
    {
        public float MoveSpeed;
        public float2 movementDirection;
    }
}
