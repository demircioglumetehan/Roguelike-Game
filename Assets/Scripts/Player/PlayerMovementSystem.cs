using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace RogueLikeGame.ECS.Player
{
    public partial class PlayerMovementSystem : SystemBase
    {
        public Action<PlayerState> OnPlayerStateChanged;

        private Entity playerEntity;
        private Entity inputEntity;
        private PlayerComponent playerComponent;
        private EntityManager entityManager;
        private InputComponent inputComponent;
        private PlayerState playerState;
        protected override void OnUpdate()
        {
            entityManager = EntityManager;
            playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
            inputEntity = SystemAPI.GetSingletonEntity<InputComponent>();
            playerComponent = entityManager.GetComponentData<PlayerComponent>(playerEntity);
            inputComponent = entityManager.GetComponentData<InputComponent>(inputEntity);
            if (math.length(inputComponent.MovementDirection) > 0)
            {
                if (playerState != PlayerState.Walking)
                {
                    playerState = PlayerState.Walking;
                    OnPlayerStateChanged?.Invoke(playerState);
                }

                Move();
            }
            else
            {
                if (playerState != PlayerState.Idle)
                {
                    playerState = PlayerState.Idle;
                    OnPlayerStateChanged?.Invoke(playerState);
                }
            }
        }

        private void Move()
        {
            LocalTransform playerTransform = entityManager.GetComponentData<LocalTransform>(playerEntity);
            PlayerComponent newPlayerComponent = entityManager.GetComponentData<PlayerComponent>(playerEntity);
            playerTransform.Position += new float3(inputComponent.MovementDirection * playerComponent.MoveSpeed * SystemAPI.Time.DeltaTime, 0);
            newPlayerComponent.movementDirection = inputComponent.MovementDirection;
            float lookDirectionAngle = inputComponent.MovementDirection.x >= 0 ? 0 : 180;

            playerTransform.Rotation = Quaternion.AngleAxis(lookDirectionAngle, Vector3.up);
            entityManager.SetComponentData(playerEntity, playerTransform);
            entityManager.SetComponentData(playerEntity, newPlayerComponent);
        }
    }
    public enum PlayerState
    {
        None = 0,
        Idle = 1,
        Walking = 2
    }
}

