using RogueLikeGame.ECS.Player;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace RogueLikeGame.ECS.BulletSystem
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct BulletMovementSystem : ISystem
    {
        private EntityManager entityManager;
        private Entity playerEntity;
        public void OnUpdate(ref SystemState state)
        {

            entityManager = state.EntityManager;
            playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
            float deltaTime = SystemAPI.Time.DeltaTime;

            var playerPosition = entityManager.GetComponentData<LocalTransform>(playerEntity).Position;
            BulletMovementSystemJob bulletMovementJob = new BulletMovementSystemJob
            {
                DeltaTime = deltaTime,
                playerPosition = playerPosition
            };
            bulletMovementJob.Schedule();

        }
        public partial struct BulletMovementSystemJob : IJobEntity
        {
            public float DeltaTime;
            public float3 playerPosition;
            public void Execute(ref LocalTransform localTransform, ref PhysicsVelocity physicsVelocity, in BulletComponent bulletComponent)
            {
                float3 direction = bulletComponent.movementDirection;

                physicsVelocity.Linear = bulletComponent.MovementSpeed * new float3(direction.x, direction.y, 0);
                physicsVelocity.Angular = float3.zero;

                float angle = math.degrees(math.atan2(direction.y, direction.x));
                localTransform.Rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                localTransform.Position.z = 0;
            }
        }
    }

}
