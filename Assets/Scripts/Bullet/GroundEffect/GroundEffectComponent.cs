using Unity.Entities;

namespace RogueLikeGame.ECS.BulletSystem.GroundEffect
{
    public struct GroundEffectComponent : IComponentData
    {
        public float LifeTime;
        public float DamageCooldown;
        public float LatestDamagedTime;
        public int Damage;
    }

}
