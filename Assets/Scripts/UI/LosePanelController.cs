using RogueLikeGame.ECS.Enemy;
using Unity.Entities;
using UnityEngine;

namespace RogueLikeGame.UI
{
    public class LosePanelController : MonoBehaviour
    {
        [SerializeField] private LosePanel losePanel;
        private void OnEnable()
        {
            var enemyDamageSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<EnemyDamageSystem>();
            enemyDamageSystem.OnPlayerDied += EnableLosePanel;
        }



        private void OnDisable()
        {
            if (World.DefaultGameObjectInjectionWorld == null)
                return;
            var enemyDamageSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<EnemyDamageSystem>();
            enemyDamageSystem.OnPlayerDied -= EnableLosePanel;

        }
        private void EnableLosePanel()
        {
            losePanel.gameObject.SetActive(true);
        }
    }

}

