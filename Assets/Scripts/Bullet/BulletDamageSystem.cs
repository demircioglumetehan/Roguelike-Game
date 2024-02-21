using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Physics;
using Unity.Transforms;
using RogueLikeGame.ECS.Enemy;

namespace RogueLikeGame.ECS.BulletSystem
{
    public partial struct BulletDamageSystem : ISystem
    {
        EntityManager entityManager;
        private void OnUpdate(ref SystemState state)
        {
            entityManager = state.EntityManager;

            NativeArray<Entity> entities = entityManager.GetAllEntities(Allocator.Temp);
            EntityCommandBuffer ECB = new EntityCommandBuffer(Allocator.Temp);
            foreach (Entity entity in entities)
            {
                if (entityManager.HasComponent<BulletComponent>(entity))
                {
                    RefRW<LocalToWorld> triggerTransform = SystemAPI.GetComponentRW<LocalToWorld>(entity);
                    RefRO<TriggerComponent> triggerComponent = SystemAPI.GetComponentRO<TriggerComponent>(entity);
                    RefRO<BulletComponent> bulletComponent = SystemAPI.GetComponentRO<BulletComponent>(entity);

                    float size = triggerComponent.ValueRO.size;
                    triggerTransform.ValueRW.Value.c0 = new float4(size, 0, 0, 0);
                    triggerTransform.ValueRW.Value.c1 = new float4(0, size, 0, 0);
                    triggerTransform.ValueRW.Value.c2 = new float4(0, 0, size, 0);

                    PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

                    NativeList<ColliderCastHit> hits = new NativeList<ColliderCastHit>(Allocator.Temp);

                    physicsWorld.SphereCastAll(triggerTransform.ValueRO.Position, triggerComponent.ValueRO.size / 2,
                        float3.zero, 1, ref hits, new CollisionFilter
                        { BelongsTo = (uint)CollisionLayer.Bullet, CollidesWith = (uint)CollisionLayer.Enemy });
                    bool bullethit = false;
                    foreach (ColliderCastHit hit in hits)
                    {

                        RefRW<EnemyComponent> enemyComponent = SystemAPI.GetComponentRW<EnemyComponent>(hit.Entity);
                        bullethit = true;
                        enemyComponent.ValueRW.CurrentHealth -= bulletComponent.ValueRO.Damage;
                        if (enemyComponent.ValueRO.CurrentHealth <= 0)
                        {
                            ECB.DestroyEntity(hit.Entity);
                        }
                    }
                    if (bullethit)
                    {

                        if (bulletComponent.ValueRO.HasGroundEffect)
                        {
                            CreateGroundEffect(ref state, ECB, bulletComponent, triggerTransform.ValueRO.Position);
                        }
                        ECB.DestroyEntity(entity);
                    }
                    hits.Dispose();

                }

            }
            ECB.Playback(entityManager);
            ECB.Dispose();
            entities.Dispose();
        }

        private void CreateGroundEffect(ref SystemState state, EntityCommandBuffer ECB, RefRO<BulletComponent> bulletComponent, float3 spawnPosition)
        {
            Entity newGroundEffect = state.EntityManager.Instantiate(bulletComponent.ValueRO.GroundEffect);
            RefRW<LocalTransform> localTransform = SystemAPI.GetComponentRW<LocalTransform>(newGroundEffect);
            localTransform.ValueRW.Position = spawnPosition;
        }
    }
}

public enum CollisionLayer
{
    Player = 1 << 6,
    Enemy = 1 << 7,
    Bullet = 1 << 8
}