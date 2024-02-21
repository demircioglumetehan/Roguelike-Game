using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace RogueLikeGame.ECS.BulletSystem.GroundEffect
{
    public class GroundEffectAuthoring : MonoBehaviour
    {
        public float LifeTime;
        public float DamageCooldown;
        public int Damage;
        public class GroundEffectBaker : Baker<GroundEffectAuthoring>
        {
            public override void Bake(GroundEffectAuthoring authoring)
            {
                Entity weaponAuthoringEntity = GetEntity(TransformUsageFlags.None);

                AddComponent(weaponAuthoringEntity, new GroundEffectComponent
                {
                    LifeTime = authoring.LifeTime,
                    Damage = authoring.Damage,
                    DamageCooldown = authoring.DamageCooldown,
                    LatestDamagedTime = (float)Time.time
                });
            }
        }
    }

}
