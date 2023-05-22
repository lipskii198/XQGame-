using _game.Scripts.ObjectPooling;
using _game.Scripts.Utility;
using UnityEngine;

namespace _game.Scripts.Managers
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

        private HudManager hudManager;
        public GameObject GetPlayerPrefab => playerPrefab;
        public WaveManager GetWaveManager { get; private set; }

        public LevelManager GetLevelManager { get; private set; }

        public HudManager GetHudManager => hudManager;
        

        protected override void Awake()
        {
            base.Awake();
            GetLevelManager = GetComponent<LevelManager>();
            hudManager = GetComponent<HudManager>();
        }


        public void OnPlayerDeath()
        {
            if (isPlayerDead) return;
            isPlayerDead = true;
            Debug.Log("Player died");
            GetLevelManager.GetPlayer.SetActive(false);
            hudManager.ShowPlayerDeathScreen();
            GetLevelManager.UnloadLevel(mainMenu: true);
            
            isPlayerDead = false;
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
            if (GetLevelManager.GetCurrentLevelData.IsWaveLevel)
            {
                GetWaveManager = GameObject.FindWithTag("WaveManager").GetComponent<WaveManager>();
            }
            ObjectPoolManager.Instance.Initialize();
            hudManager.Initialize();
        }

        public void OnLevelUnloaded()
        {
            isInLevel = false;
            Debug.Log("Cleaning up level data");
            GetLevelManager.Reset();
            hudManager.Reset();
        }
        
    }
}