using Unity.Entities;

namespace RogueLikeGame.ECS.Enemy
{

    public struct EnemyComponent : IComponentData
    {
        public float CurrentHealth;
        public float AvailableAttackTime;
        public EnemyData EnemyData;
    }

}
