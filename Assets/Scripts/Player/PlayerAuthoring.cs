using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace RogueLikeGame.ECS.Player
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public float MoveSpeed;
        public float2 movementDirection;
        public int PlayerHealth;
        public class PlayerBaker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                Entity playerEntity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(playerEntity, new PlayerComponent
                {
                    MoveSpeed = authoring.MoveSpeed,
                    movementDirection = authoring.movementDirection
                });
                AddComponent(playerEntity, new PlayerHealthComonent
                {
                    CurrentHealth = authoring.PlayerHealth,
                    MaximumHealth = authoring.PlayerHealth,
                });

            }
        }
    }

}
