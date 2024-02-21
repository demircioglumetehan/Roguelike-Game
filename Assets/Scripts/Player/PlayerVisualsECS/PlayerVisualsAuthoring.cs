using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace RogueLikeGame.ECS.Player.Visuals
{
    public class PlayerVisualsAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        public class Baker : Baker<PlayerVisualsAuthoring>
        {
            public override void Bake(PlayerVisualsAuthoring authoring)
            {
                Entity playerPrefabEntity = GetEntity(TransformUsageFlags.None);
                AddComponentObject(playerPrefabEntity, new PlayerVisualsData
                {
                    Player = authoring.playerPrefab
                });
            }
        }
    }

}
