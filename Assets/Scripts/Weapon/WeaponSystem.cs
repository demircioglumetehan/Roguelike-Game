using Unity.Entities;
using Random = Unity.Mathematics.Random;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using RogueLikeGame.ECS.BulletSystem;
using RogueLikeGame.ECS.Player;

namespace RogueLikeGame.ECS.WeaponSystem
{
    public partial struct WeaponSystem : ISystem
    {
        private Entity playerEntity;
        private LocalTransform playerTransformComponent;
        private EntityManager entityManager;
        EntityCommandBuffer ECB;
        private Random random;

        private PlayerComponent playerComponent;
        public void OnCreate(ref SystemState state)
        {
            random = Random.CreateFromIndex((uint)1000);
        }
        public void OnUpdate(ref SystemState state)
        {

            if (!SystemAPI.TryGetSingletonEntity<PlayerComponent>(out playerEntity))
            {
                return;
            }
            entityManager = state.EntityManager;
            playerTransformComponent = entityManager.GetComponentData<LocalTransform>(playerEntity);
            playerComponent = entityManager.GetComponentData<PlayerComponent>(playerEntity);
            ECB = new EntityCommandBuffer(Allocator.Temp);
            foreach ((RefRW<WeaponComponent> weaponComponent, Entity entity) in SystemAPI.Query<RefRW<WeaponComponent>>().WithEntityAccess())
            {
                if (SystemAPI.Time.ElapsedTime > weaponComponent.ValueRO.LatestShootTime)
                {
                    weaponComponent.ValueRW.LatestShootTime = (float)SystemAPI.Time.ElapsedTime + weaponComponent.ValueRO.Cooldown;
                    SpawnWeaponBullet(weaponComponent.ValueRO);
                }

            }
            ECB.Playback(entityManager);
            ECB.Dispose();

        }
        private void SpawnWeaponBullet(WeaponComponent weaponData)
        {
            for (int i = 0; i < weaponData.ProjectileCount; i++)
            {
                Entity newBullet = entityManager.Instantiate(weaponData.BulletPrefab);
                entityManager.SetComponentData(newBullet, new LocalTransform
                {
                    Position = playerTransformComponent.Position,
                    Rotation = quaternion.identity,
                    Scale = 1
                });
                float3 movementDir;
                bool hasGroundEffect;
                if (weaponData.FireType == FireType.Random)
                {
                    movementDir = GetRandomDirection();
                    hasGroundEffect = false;
                }
                else
                {
                    movementDir = new float3(playerComponent.movementDirection.x, playerComponent.movementDirection.y, 0);
                    movementDir = math.normalize(movementDir);
                    hasGroundEffect = true;

                }
                var newbulletComponent = new BulletComponent
                {
                    Damage = weaponData.Damage,
                    MovementSpeed = weaponData.ProjectileSpeed,
                    movementDirection = movementDir,
                    HasGroundEffect = hasGroundEffect,
                    GroundEffect = weaponData.GroundEffectPrefab
                };
                ECB.AddComponent<BulletComponent>(newBullet);
                ECB.SetComponent(newBullet, newbulletComponent);
            }

        }
        private float3 GetRandomDirection()
        {
            float3 direction = new float3(random.NextFloat2(-1, 1), 0);
            direction = math.normalize(direction);
            return direction;
        }

    }

}
