using UnityEngine;

namespace _game.Scripts.ScriptableObjects
{
    public class EnemyBaseData : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private int health;
        [SerializeField] private int damage; // Melee damage
        [SerializeField] private int movementSpeed;
        [SerializeField] private int attackSpeed;
        [SerializeField] private int attackRange;
        [SerializeField] private int detectionRange;
        
        public int Id => id;
        public int Health => health;
        public int Damage => damage;
        public int MovementSpeed => movementSpeed;
        public int AttackSpeed => attackSpeed;
        public int AttackRange => attackRange;
        public int DetectionRange => detectionRange;
    }
}