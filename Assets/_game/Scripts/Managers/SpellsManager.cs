﻿using System.Collections.Generic;
using _game.Scripts.ObjectPooling;
using _game.Scripts.ScriptableObjects;
using UnityEngine;

namespace _game.Scripts.Managers
{
    public class SpellsManager : MonoBehaviour
    {
        [SerializeField] private ProjectileData currentProjectile;
        [SerializeField] private Transform projectileSpawnPoint;
        private Dictionary<string, ProjectileData> spells;


        private void Start()
        {
            spells = new Dictionary<string, ProjectileData>();
            AddSpell("Fireball", Resources.Load<ProjectileData>("ScriptableObjects/Spells/Fireball"));
            currentProjectile = spells["Fireball"];
            
            Debug.Log($"[{GetType().Name}] Initialized");
        }

        public void Cast(string spellName)
        {
            if (!spells.ContainsKey(spellName))
                Debug.LogError($"Spell {spellName} not found");
            
            
            var spellObj = ObjectPoolManager.Instance.GetPooledObject("PlayerProjectile");
            if (spellObj == null)
            {
                Debug.LogError("No pooled objects available");
                return;
            }
            spellObj.transform.position = projectileSpawnPoint.position;
            spellObj.GetComponent<Projectile>().Cast(Mathf.Sign(transform.localScale.x), spells[spellName], gameObject);
        }
        
        public void AddSpell(string spellName, ProjectileData projectile)
        {
            if(spells.ContainsKey(spellName))
            {
                Debug.LogError("Spell already added.");
                return;
            }
            spells.Add(spellName, projectile);
        }

        public ProjectileData GetCurrentProjectile => currentProjectile;
    }
}