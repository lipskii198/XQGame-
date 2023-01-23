using System;
using System.Collections.Generic;
using ObjectPooling;
using ScriptableObjects;
using UnityEngine;

namespace Managers
{
    public class SpellsManager : MonoBehaviour
    {
        [SerializeField] private SpellData currentSpell;
        [SerializeField] private Transform projectileSpawnPoint;
        private Dictionary<string, SpellData> spells;


        private void Start()
        {
            spells = new Dictionary<string, SpellData>();
            AddSpell("Fireball", Resources.Load<SpellData>("ScriptableObjects/Spells/Fireball"));
        }

        public void Cast(string spellName)
        {
            if (!spells.ContainsKey(spellName))
                Debug.LogError($"Spell {spellName} not found");
            
            
            var spellObj = ObjectPoolManager.Instance.GetPooledObject("PlayerProjectile");
            spellObj.transform.position = projectileSpawnPoint.position;
            spellObj.GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x), spells[spellName]);
        }
        
        public void AddSpell(string spellName, SpellData spell)
        {
            if(spells.ContainsKey(spellName))
            {
                Debug.LogError("Spell already added.");
                return;
            }
            spells.Add(spellName, spell);
        }

        public void ChangeSpell(SpellData spell)
        {
            currentSpell = spell;
            //ObjectPoolManager.Instance.UpdatePooledObjects(currentSpell.SpellPrefab);
        }
    
        public SpellData GetCurrentSpell => currentSpell;
    }
}