
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using RogueLikeGame.ECS.Player;

namespace RogueLikeGame.SpriteSheetAnimation.ECS
{
    public struct SpriteSheetAnimationData : IComponentData
    {
        public int currentFrame;
        public int frameCount;
        public float frameTimer;
        public float frameTimerMax;
        public int materialIndex;
        public Vector4 uv;
        public Matrix4x4 matrix;
    }


    public partial struct SpriteSheetAnimationAnimateSystem : ISystem
    {

        private EntityManager entityManager;
        private Entity playerEntity;
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {

            entityManager = state.EntityManager;
            playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
            float deltaTime = SystemAPI.Time.DeltaTime;
            SpriteSheetAnimationJob enemyaiJob = new SpriteSheetAnimationJob
            {
                DeltaTime = deltaTime
            };
            enemyaiJob.Schedule();

        }


        public partial struct SpriteSheetAnimationJob : IJobEntity
        {
            public float DeltaTime;
            public void Execute(ref SpriteSheetAnimationData spriteSheetAnimationData, ref LocalTransform translation)
            {
                spriteSheetAnimationData.frameTimer += DeltaTime;
                while (spriteSheetAnimationData.frameTimer >= spriteSheetAnimationData.frameTimerMax)
                {
                    spriteSheetAnimationData.frameTimer -= spriteSheetAnimationData.frameTimerMax;
                    spriteSheetAnimationData.currentFrame = (spriteSheetAnimationData.currentFrame + 1) % spriteSheetAnimationData.frameCount;

                    float uvWidth = 1f / spriteSheetAnimationData.frameCount;
                    float uvHeight = 1f;
                    float uvOffsetX = uvWidth * spriteSheetAnimationData.currentFrame;
                    float uvOffsetY = 0f;
                    spriteSheetAnimationData.uv = new Vector4(uvWidth, uvHeight, uvOffsetX, uvOffsetY);

                    float3 position = translation.Position;
                    position.z = position.y * .01f;
                    spriteSheetAnimationData.matrix = Matrix4x4.TRS(position, translation.Rotation, translation.Scale * Vector3.one);
                }
            }
        }


    }
}
