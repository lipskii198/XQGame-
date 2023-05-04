using UnityEngine;

namespace _game.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObjects/ProjectileData", order = 0)]
    public class ProjectileData : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private string projectileName;
        [SerializeField] private string description;
        [SerializeField] private float speed;
        [SerializeField] private float timeToLive;
        [SerializeField] private float damage;
        [SerializeField] private float manaCost;
        [SerializeField] private float cooldown;
        [SerializeField] private GameObject spellPrefab;


        public int Id => id;
        public string ProjectileName => projectileName;
        public string Description => description;
        public float Speed => speed; public float TimeToLive => timeToLive;
        public float Damage => damage;
        public float ManaCost => manaCost;
        public float Cooldown => cooldown;
        public GameObject SpellPrefab => spellPrefab;
    }
}