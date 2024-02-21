
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Burst;
using RogueLikeGame.ECS.Player;

namespace RogueLikeGame.ECS.Enemy
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [BurstCompile]
    public partial struct EnemyAISystem : ISystem
    {
        private EntityManager entityManager;
        private Entity playerEntity;
        [BurstCompile]
        private void OnUpdate(ref SystemState state)
        {
            entityManager = state.EntityManager;
            playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
            float deltaTime = SystemAPI.Time.DeltaTime;

            var playerPosition = entityManager.GetComponentData<LocalTransform>(playerEntity).Position;
            EnemyAiSystemJob enemyaiJob = new EnemyAiSystemJob
            {
                DeltaTime = deltaTime,
                playerPosition = playerPosition
            };
            enemyaiJob.Schedule();

        }
        public partial struct EnemyAiSystemJob : IJobEntity
        {
            public float DeltaTime;
            public float3 playerPosition;
            public void Execute(ref LocalTransform localTransform, ref PhysicsVelocity physicsVelocity, in EnemyComponent enemyComp)
            {
                float3 direction = playerPosition - localTransform.Position;
                direction = math.normalize(direction);

                physicsVelocity.Linear = enemyComp.EnemyData.MoveSpeed * DeltaTime * new float3(direction.x, direction.y, 0);
                physicsVelocity.Angular = float3.zero;

                localTransform.Rotation = quaternion.Euler(new float3(0, 0, 0));
                localTransform.Position.z = 0;
            }
        }
    }

}
