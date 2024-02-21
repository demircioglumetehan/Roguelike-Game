using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace RogueLikeGame.ECS.Enemy.Spawner
{
    using Random = Unity.Mathematics.Random;
    using UnityEngine;
    public partial class EnemySpawnerSystem : SystemBase
    {
        private RefRW<EnemySpawnerComponent> enemySpawnerComponent;
        private EnemyDataContainer enemyDataContainerComponent;
        private Entity enemySpawnerEntity;
        private float nextSpawnTime;
        private Random random;

        protected override void OnCreate()
        {
            random = Random.CreateFromIndex((uint)enemySpawnerComponent.GetHashCode());
        }
        protected override void OnUpdate()
        {
            if (!SystemAPI.TryGetSingletonEntity<EnemySpawnerComponent>(out enemySpawnerEntity))
            {
                return;
            }
            if (nextSpawnTime > SystemAPI.Time.ElapsedTime)
            {
                return;

            }
            enemySpawnerComponent = SystemAPI.GetSingletonRW<EnemySpawnerComponent>();
            enemyDataContainerComponent = EntityManager.GetComponentObject<EnemyDataContainer>(enemySpawnerEntity);
            nextSpawnTime = (float)SystemAPI.Time.ElapsedTime + enemySpawnerComponent.ValueRO.spawnCooldown;
            if (enemySpawnerComponent.ValueRO.spawningAmountMob1 > 0)
            {
                for (int i = 0; i < enemySpawnerComponent.ValueRO.spawningEnemyAtOnce; i++)
                {
                    enemySpawnerComponent.ValueRW.spawningAmountMob1--;
                    SpawnSpesificEnemy(enemyDataContainerComponent.enemies[0]);
                }
            }
            else if (enemySpawnerComponent.ValueRO.spawningAmountMob2 > 0)
            {
                for (int i = 0; i < enemySpawnerComponent.ValueRO.spawningEnemyAtOnce; i++)
                {
                    enemySpawnerComponent.ValueRW.spawningAmountMob2--;
                    SpawnSpesificEnemy(enemyDataContainerComponent.enemies[1]);
                }
            }
        }

        private void SpawnSpesificEnemy(EnemyData enemyData)
        {
            Entity newEnemy = EntityManager.Instantiate(enemyData.EnemyPrefab);
            EntityManager.SetComponentData(newEnemy, new LocalTransform
            {
                Position = GetPositionOutsideOfCameraRange(),
                Rotation = quaternion.identity,
                Scale = 1
            });
        }

        private float3 GetPositionOutsideOfCameraRange()
        {
            float3 position = new float3(random.NextFloat2(-enemySpawnerComponent.ValueRO.cameraSize * 4, enemySpawnerComponent.ValueRO.cameraSize * 4), 0);

            while (position.x < enemySpawnerComponent.ValueRO.cameraSize.x && position.x > -enemySpawnerComponent.ValueRO.cameraSize.x
                && position.y < enemySpawnerComponent.ValueRO.cameraSize.y && position.y > -enemySpawnerComponent.ValueRO.cameraSize.y)
            {
                position = new float3(random.NextFloat2(-enemySpawnerComponent.ValueRO.cameraSize * 4, enemySpawnerComponent.ValueRO.cameraSize * 4), 0);
            }

            position += new float3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);

            return position;
        }
    }
}
