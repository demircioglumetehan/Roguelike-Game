using RogueLikeGame.ECS.Enemy;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace RogueLikeGame.UI
{
    public class PlayerHealthBar : MonoBehaviour
    {
        Slider slider;
        private void Awake()
        {
            slider = GetComponent<Slider>();
            slider.value = 1f;
        }
        private void OnEnable()
        {
            var enemyDamageSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<EnemyDamageSystem>();
            enemyDamageSystem.OnPlayerHealthChanged += UpdateHealthBar;

        }
        private void OnDisable()
        {
            if (World.DefaultGameObjectInjectionWorld == null)
                return;
            var enemyDamageSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<EnemyDamageSystem>();
            enemyDamageSystem.OnPlayerHealthChanged -= UpdateHealthBar;
        }
        private void UpdateHealthBar(int currentHealth, int maximumHealth)
        {

            slider.value = (float)currentHealth / (float)maximumHealth;
        }
    }

}
