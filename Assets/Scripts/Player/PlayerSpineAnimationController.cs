using RogueLikeGame.ECS.Player;
using Spine.Unity;
using Unity.Entities;
using UnityEngine;

namespace RogueLikeGame.Player.Visuals
{
    public class PlayerSpineAnimationController : MonoBehaviour
    {
        SkeletonAnimation skeletonAnimation;
        private void OnEnable()
        {
            var playerSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<PlayerMovementSystem>();
            playerSystem.OnPlayerStateChanged += ChangeAnimation;
        }

        private void OnDisable()
        {
            if (World.DefaultGameObjectInjectionWorld == null)
                return;
            var playerSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<PlayerMovementSystem>();
            playerSystem.OnPlayerStateChanged -= ChangeAnimation;

        }
        private void Awake()
        {
            skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
            skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
        }

        private void ChangeAnimation(PlayerState state)
        {
            switch (state)
            {
                case PlayerState.None:
                    Debug.Log("None");
                    break;
                case PlayerState.Idle:
                    skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                    Debug.Log("Idle");
                    break;
                case PlayerState.Walking:
                    skeletonAnimation.AnimationState.SetAnimation(0, "Move", true);
                    Debug.Log("Walking");
                    break;
                default:
                    break;
            }
        }
    }

}

