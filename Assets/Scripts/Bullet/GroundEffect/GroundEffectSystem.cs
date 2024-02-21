using System.Collections;
using System.Collections.Generic;
using RogueLikeGame.ECS.Enemy;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
namespace RogueLikeGame.ECS.BulletSystem.GroundEffect
{
    public partial class GroundEffectSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            EntityCommandBuffer ECB = new EntityCommandBuffer(Allocator.Temp);

            foreach ((RefRW<LocalToWorld> triggerTransform, RefRO<TriggerComponent> triggerComponent, RefRW<GroundEffectComponent> groundEffectComponent, Entity entity) in SystemAPI.Query<RefRW<LocalToWorld>, RefRO<TriggerComponent>, RefRW<GroundEffectComponent>>().WithEntityAccess())
            {
                groundEffectComponent.ValueRW.LifeTime -= SystemAPI.Time.DeltaTime;
                if (groundEffectComponent.ValueRO.LifeTime <= 0)
                {
                    ECB.DestroyEntity(entity);
                    continue;
                }

                if (groundEffectComponent.ValueRW.LatestDamagedTime + groundEffectComponent.ValueRW.DamageCooldown > (float)SystemAPI.Time.ElapsedTime)
                {
                    continue;
                }

                PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
                NativeList<ColliderCastHit> hits = new NativeList<ColliderCastHit>(Allocator.Temp);

                physicsWorld.SphereCastAll(triggerTransform.ValueRO.Position, triggerComponent.ValueRO.size / 2,
                        float3.zero, 1, ref hits, new CollisionFilter
                        { BelongsTo = (uint)CollisionLayer.Bullet, CollidesWith = (uint)CollisionLayer.Enemy });
                bool damaged = false;
                foreach (ColliderCastHit hit in hits)
                {
                    if (hit.Entity == null)
                        continue;
                    RefRW<EnemyComponent> enemyComponent = SystemAPI.GetComponentRW<EnemyComponent>(hit.Entity);

                    damaged = true;
                    enemyComponent.ValueRW.CurrentHealth -= groundEffectComponent.ValueRO.Damage;
                    if (enemyComponent.ValueRO.CurrentHealth <= 0)
                    {
                        ECB.DestroyEntity(hit.Entity);
                    }


                }
                if (damaged)
                {
                    groundEffectComponent.ValueRW.LatestDamagedTime = (float)SystemAPI.Time.ElapsedTime;

                }
            }
            ECB.Playback(EntityManager);
            ECB.Dispose();
        }


    }
}

