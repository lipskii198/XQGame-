using ScriptableObjects;
using UnityEngine;

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
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private LevelData currentLevelData;
        
        private WaveManager waveManager;
        public LevelData GetCurrentLevelData => currentLevelData;
    
        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            
            waveManager = GetComponent<WaveManager>();
        
            SetCurrentLevelData("AntsLevel");
        
            if (currentLevelData == null)
            {
                Debug.LogError("Current level data is null");
                return;
            }

            Debug.Log($"Current level: {currentLevelData.LevelName} - {currentLevelData.LevelNumber} - IsBossLevel: {currentLevelData.IsBossLevel} - IsWaveLevel: {currentLevelData.IsWaveLevel}");
        
            if(currentLevelData.IsBossLevel)
                Debug.Log("Boss level");
            else if(currentLevelData.IsWaveLevel)
                waveManager.enabled = true;
            else
                Debug.Log("Normal level");
        }

        //TODO: Change this to a dictionary
        public void SetCurrentLevelData(string levelName)
        {
            currentLevelData = Resources.Load<LevelData>($"ScriptableObjects/Levels/LD_{levelName}");
        }
        
        public void OnLevelLoaded()
        {
            var spawnPoint = GameObject.FindWithTag("Respawn");
            player = Instantiate(playerPrefab, spawnPoint.transform.position, Quaternion.identity);
        }
        
        public void WinCurrentLevel()
        {
            Debug.Log("Win");
        }
        
        public void LoseCurrentLevel()
        {
            Debug.Log("Lose");
        }
    
        public void OnPlayerDeath()
        {
            Debug.Log("Player died");
            player.SetActive(false);
        }
    }
}