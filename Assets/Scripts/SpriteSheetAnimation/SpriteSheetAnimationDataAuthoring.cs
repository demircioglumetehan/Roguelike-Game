using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace RogueLikeGame.SpriteSheetAnimation.ECS
{
    public class SpriteSheetAnimationDataAuthoring : MonoBehaviour
    {
        public int currentFrame;
        public int frameCount;
        public float frameTimer;
        public float frameTimerMax;
        public int materialIndex;

        public Vector4 uv;
        public Matrix4x4 matrix;
        public class SpriteSheetAnimationBaker : Baker<SpriteSheetAnimationDataAuthoring>
        {
            public override void Bake(SpriteSheetAnimationDataAuthoring authoring)
            {
                Entity spriteSheetAnimationAuthoringEntity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(spriteSheetAnimationAuthoringEntity, new SpriteSheetAnimationData
                {
                    currentFrame = UnityEngine.Random.Range(0, authoring.frameCount),
                    frameCount = authoring.frameCount,
                    frameTimer = UnityEngine.Random.Range(0f, 1f),
                    frameTimerMax = authoring.frameTimerMax,
                    materialIndex = authoring.materialIndex


                }); ;
            }
        }
    }

}
