using UnityEngine;

namespace _game.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 0)]
    public class EnemyData : EnemyBaseData
    {
        [SerializeField] private string enemyName;

        public string EnemyName => enemyName;
    }
}