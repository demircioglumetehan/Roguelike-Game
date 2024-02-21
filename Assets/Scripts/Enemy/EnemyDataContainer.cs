using System.Collections.Generic;
using Unity.Entities;

namespace RogueLikeGame.ECS.Enemy
{
    public class EnemyDataContainer : IComponentData
    {
        public List<EnemyData> enemies;
    }
    [System.Serializable]
    public struct EnemyData
    {
        public float Health;
        public int Damage;
        public float MoveSpeed;
        public float DamageCooldown;
        public Entity EnemyPrefab;

    }
}
