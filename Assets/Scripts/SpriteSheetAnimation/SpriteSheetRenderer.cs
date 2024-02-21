
using Unity.Entities;
using Unity.Transforms;

namespace RogueLikeGame.SpriteSheetAnimation.ECS
{
    using UnityEngine;
    [UpdateAfter(typeof(SpriteSheetAnimationAnimateSystem))]
    public partial class SpriteSheetRenderer : SystemBase
    {

        protected override void OnUpdate()
        {
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            Vector4[] uv = new Vector4[1];
            Camera camera = Camera.main;
            Mesh quadMesh = SpriteSheetHandler.Instance.QuadMesh;
            foreach ((RefRO<LocalTransform> localTransform, RefRW<SpriteSheetAnimationData> spriteSheetAnimationData, Entity entity) in SystemAPI.Query<RefRO<LocalTransform>, RefRW<SpriteSheetAnimationData>>().WithEntityAccess())
            {
                Material material = SpriteSheetHandler.Instance.WalkingSpriteSheetMaterial[spriteSheetAnimationData.ValueRO.materialIndex];
                uv[0] = spriteSheetAnimationData.ValueRO.uv;
                materialPropertyBlock.SetVectorArray("_MainTex_UV", uv);
                Graphics.DrawMesh(
                    quadMesh,
                    spriteSheetAnimationData.ValueRO.matrix,
                    material,
                    0, // Layer
                    camera,
                    0, // Submesh index
                    materialPropertyBlock
                );
            };

        }
    }

}
