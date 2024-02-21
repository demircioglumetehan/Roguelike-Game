using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
namespace RogueLikeGame.ECS.Enemy
{
    public class EnemyDataAuthoring : MonoBehaviour
    {
        public EnemyData enemyData;
        public class EnemyDataBaker : Baker<EnemyDataAuthoring>
        {
            public override void Bake(EnemyDataAuthoring authoring)
            {
                Entity enemyEntity = GetEntity(TransformUsageFlags.None);
                AddComponent(enemyEntity, new EnemyComponent
                {
                    EnemyData = authoring.enemyData,
                    CurrentHealth = authoring.enemyData.Health
                });

            }
        }
    }

}
