using System;
using RogueLikeGame.ECS.Player;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace RogueLikeGame.ECS.Enemy
{
    public partial class EnemyDamageSystem : SystemBase
    {
        public Action<int, int> OnPlayerHealthChanged;
        public Action OnPlayerDied;
        protected override void OnUpdate()
        {
            EntityManager entityManager = EntityManager;

            NativeArray<Entity> entities = entityManager.GetAllEntities(Allocator.Temp);
            EntityCommandBuffer ECB = new EntityCommandBuffer(Allocator.Temp);
            var playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();

            var playerPosition = entityManager.GetComponentData<LocalTransform>(playerEntity).Position;
            foreach ((RefRW<LocalToWorld> triggerTransform, RefRO<TriggerComponent> triggerComponent, RefRW<EnemyComponent> enemyComponent, Entity entity) in SystemAPI.Query<RefRW<LocalToWorld>, RefRO<TriggerComponent>, RefRW<EnemyComponent>>().WithEntityAccess())
            {
                if (enemyComponent.ValueRO.AvailableAttackTime > SystemAPI.Time.ElapsedTime)
                {
                    continue;
                }
                if (math.distance(playerPosition, triggerTransform.ValueRO.Position) > 2) { continue; }
                float size = triggerComponent.ValueRO.size;
                triggerTransform.ValueRW.Value.c0 = new float4(size, 0, 0, 0);
                triggerTransform.ValueRW.Value.c1 = new float4(0, size, 0, 0);
                triggerTransform.ValueRW.Value.c2 = new float4(0, 0, size, 0);

                PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

                NativeList<ColliderCastHit> hits = new NativeList<ColliderCastHit>(Allocator.Temp);

                physicsWorld.SphereCastAll(triggerTransform.ValueRO.Position, triggerComponent.ValueRO.size / 2,
                    float3.zero, 1, ref hits, new CollisionFilter
                    { BelongsTo = (uint)CollisionLayer.Enemy, CollidesWith = (uint)CollisionLayer.Player });

                foreach (ColliderCastHit hit in hits)
                {

                    RefRW<PlayerHealthComonent> playerHealthComponent = SystemAPI.GetComponentRW<PlayerHealthComonent>(hit.Entity);

                    playerHealthComponent.ValueRW.CurrentHealth -= enemyComponent.ValueRO.EnemyData.Damage;

                    if (playerHealthComponent.ValueRO.CurrentHealth <= 0)
                    {
                        playerHealthComponent.ValueRW.CurrentHealth = 0;
                        OnPlayerDied?.Invoke();
                        Enabled = false;
                    }
                    OnPlayerHealthChanged?.Invoke(playerHealthComponent.ValueRO.CurrentHealth, playerHealthComponent.ValueRO.MaximumHealth);
                    enemyComponent.ValueRW.AvailableAttackTime = (float)SystemAPI.Time.ElapsedTime + enemyComponent.ValueRO.EnemyData.DamageCooldown;
                }


                hits.Dispose();
            }

            ECB.Playback(entityManager);
            ECB.Dispose();
            entities.Dispose();
        }

        
    }
}

