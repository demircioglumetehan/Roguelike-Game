using Unity.Entities;

namespace RogueLikeGame.ECS.WeaponSystem
{
    public struct WeaponComponent : IComponentData
    {
        public float LatestShootTime;
        public int ProjectileCount;
        public float Cooldown;
        public int ProjectileSpeed;
        public int Damage;
        public Entity BulletPrefab;
        public Entity GroundEffectPrefab;
        public FireType FireType;

    }

}
