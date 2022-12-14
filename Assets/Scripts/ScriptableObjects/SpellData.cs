using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SpellData", menuName = "ScriptableObjects/SpellData", order = 0)]
    public class SpellData : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private string spellName;
        [SerializeField] private string description;
        [SerializeField] private float speed;
        [SerializeField] private float timeToLive;
        [SerializeField] private float damage;
        [SerializeField] private float manaCost;
        [SerializeField] private float cooldown;
        [SerializeField] private GameObject spellPrefab;


        public int Id => id;
        public string SpellName => spellName;
        public string Description => description;
        public float Speed => speed;
        public float TimeToLive => timeToLive;
        public float Damage => damage;
        public float ManaCost => manaCost;
        public float Cooldown => cooldown;
        public GameObject SpellPrefab => spellPrefab;
    }
}