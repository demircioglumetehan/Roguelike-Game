using Unity.Entities;
using Unity.Mathematics;

namespace RogueLikeGame.ECS.Enemy.Spawner
{
    public struct EnemySpawnerComponent : IComponentData
    {
        public float spawnCooldown;
        public int spawningAmountMob1;
        public int spawningAmountMob2;
        public int spawningEnemyAtOnce;
        public float2 cameraSize;
    }
}
