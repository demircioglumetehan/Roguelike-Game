using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;
using RogueLikeGame.ScriptableObjects.Enemy;

namespace RogueLikeGame.ECS.Enemy.Spawner
{
    public class EnemySpawnerAuthoring : MonoBehaviour
    {
        public float spawnCooldown = 1;
        public int spawningEnemyAtOnce = 1;
        public Vector2 cameraSize;
        public int spawningAmountMob1;
        public int spawningAmountMob2;
        public List<EnemyScriptableObject> enemiesSO;



        public class EnemySpawnerBaker : Baker<EnemySpawnerAuthoring>
        {
            public override void Bake(EnemySpawnerAuthoring authoring)
            {
                Entity enemySpawnerAuthoring = GetEntity(TransformUsageFlags.None);

                AddComponent(enemySpawnerAuthoring, new EnemySpawnerComponent
                {
                    spawnCooldown = authoring.spawnCooldown,
                    cameraSize = authoring.cameraSize,
                    spawningAmountMob1 = authoring.spawningAmountMob1,
                    spawningAmountMob2 = authoring.spawningAmountMob2,
                    spawningEnemyAtOnce = authoring.spawningEnemyAtOnce,

                });

                List<EnemyData> enemyData = new List<EnemyData>();

                foreach (EnemyScriptableObject enemySO in authoring.enemiesSO)
                {
                    enemyData.Add(new EnemyData
                    {
                        Damage = enemySO.Damage,
                        Health = enemySO.Health,
                        MoveSpeed = enemySO.MoveSpeed,
                        DamageCooldown = enemySO.DamageCooldown,
                        EnemyPrefab = GetEntity(enemySO.EnemyPrefab, TransformUsageFlags.None)
                    });
                }

                AddComponentObject(enemySpawnerAuthoring, new EnemyDataContainer { enemies = enemyData });
            }
        }
    }

}
