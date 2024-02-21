using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace RogueLikeGame.ECS.Player.Visuals
{
    public partial struct PlayerVisualSyncronizeSystem : ISystem
    {
        private EntityManager entityManager;
        private void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.ManagedAPI.TryGetSingleton(out PlayerVisualsData animationVisualsPrefabs))
            {
                return;
            }
            entityManager = state.EntityManager;
            EntityCommandBuffer ECB = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (transform, playerComponent, entity) in SystemAPI.Query<LocalTransform, PlayerComponent>().WithEntityAccess())
            {
                if (!entityManager.HasComponent<VisualsReferenceComponent>(entity))
                {
                    GameObject playerVisuals = Object.Instantiate(animationVisualsPrefabs.Player);
                    ECB.AddComponent(entity, new VisualsReferenceComponent { gameObject = playerVisuals });

                }
                else
                {
                    VisualsReferenceComponent playerVisualsReference = entityManager.GetComponentData<VisualsReferenceComponent>(entity);
                    playerVisualsReference.gameObject.transform.position = transform.Position;
                    playerVisualsReference.gameObject.transform.rotation = transform.Rotation;
                    InputComponent inputComponent = SystemAPI.GetSingleton<InputComponent>();
                }
            }
            ECB.Playback(entityManager);
            ECB.Dispose();
        }
    }
    public class VisualsReferenceComponent : IComponentData
    {
        public GameObject gameObject;
    }
}
