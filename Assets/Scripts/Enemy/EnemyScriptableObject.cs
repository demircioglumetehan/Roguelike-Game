using UnityEngine;

namespace RogueLikeGame.ScriptableObjects.Enemy
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy")]
    public class EnemyScriptableObject : ScriptableObject
    {
        [field: SerializeField] public float Health { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public float DamageCooldown { get; private set; }
        [field: SerializeField] public GameObject EnemyPrefab { get; private set; }
    }

}
