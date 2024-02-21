using Unity.Entities;
using UnityEngine;
namespace RogueLikeGame.ECS.WeaponSystem
{
    public class WeaponAuthoring : MonoBehaviour
    {
        public float LatestShootTime;
        public int ProjectileCount;
        public float Cooldown;
        public int ProjectileSpeed;
        public int Damage;
        public GameObject BulletPrefab;
        public GameObject GroundEffectPrefab;
        public FireType FireType;

        public class WeaponBaker : Baker<WeaponAuthoring>
        {
            public override void Bake(WeaponAuthoring authoring)
            {
                Entity weaponAuthoringEntity = GetEntity(TransformUsageFlags.None);
                AddComponent(weaponAuthoringEntity, new WeaponComponent
                {
                    ProjectileCount = authoring.ProjectileCount,
                    Cooldown = authoring.Cooldown,
                    ProjectileSpeed = authoring.ProjectileSpeed,
                    Damage = authoring.Damage,
                    BulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.None),
                    GroundEffectPrefab = GetEntity(authoring.GroundEffectPrefab, TransformUsageFlags.None),
                    FireType = authoring.FireType
                }); ;
            }
        }
    }

    public enum FireType
    {
        None = 0,
        Directional = 1,
        Random = 2
    }
}
