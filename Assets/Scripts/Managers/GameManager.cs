using System;
using Enemies.Core;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    
    /*
     * TODO: Rework this system into something like this:
     * LevelManager - Handles level data, level loading, level unloading (Basically scene management)
     * WaveManager - Handles wave data, wave spawning (Done?)
     * MenuManager - Handles menu loading, pausing, etc.
     * GameManager - Handles game data, game saving/loading, game win/lose conditions
     */
    public class GameManager : LazySingletonMono<GameManager>
    {
        [SerializeField] private bool isGamePaused;
        [SerializeField] private bool isInLevel;
        [SerializeField] private bool isInCombat;
        [SerializeField] private bool isInCutscene;
        [SerializeField] private bool isPlayerDead;
        [SerializeField] private GameObject playerPrefab;
        
        private WaveManager waveManager;
        private LevelManager levelManager;
        public GameObject GetPlayerPrefab => playerPrefab;
        public WaveManager GetWaveManager => waveManager;
        public LevelManager GetLevelManager => levelManager;
        private void Start()
        {
            waveManager = GetComponent<WaveManager>();
            levelManager = GetComponent<LevelManager>();
        }
        

        public void OnPlayerDeath()
        {
            if (isPlayerDead) return;
            isPlayerDead = true;
            Debug.Log("Player died");
            levelManager.GetPlayer.SetActive(false);
        }
        
        public void OnPlayerEnterCombat()
        {
            if (isInCombat) return;
            isInCombat = true;
            Debug.Log("Player entered combat");
        }
        
        public void OnPlayerExitCombat()
        {
            if (!isInCombat) return;
            isInCombat = false;
            Debug.Log("Player exited combat");
        }
        
        public void OnLevelLoaded()
        {
            if (isInLevel) return;
            isInLevel = true;
            Debug.Log("Level loaded");
        }
        
        public void OnLevelUnloaded()
        {
            if (!isInLevel) return;
            isInLevel = false;
            Debug.Log("Level unloaded");
        }
    }
}