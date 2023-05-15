using System;
using System.Collections;
using _game.Scripts.ObjectPooling;
using _game.Scripts.ScriptableObjects;
using _game.Scripts.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _game.Scripts.Managers
{
    public class LevelManager : LazySingletonMono<LevelManager>
    {
        [SerializeField] private int failedLevelCount;
        [SerializeField] private bool isLevelLoaded;
        [SerializeField] private GameObject player;
        [SerializeField] private LevelData currentLevelData;
        
        private WaveManager waveManager;
        private AsyncOperation levelLoadingOperation;
        
        public LevelData GetCurrentLevelData => currentLevelData;
        public AsyncOperation GetLevelLoadingOperation => levelLoadingOperation;
        public GameObject GetPlayer => player;
        
        private void Start()
        {
            waveManager = GetComponent<WaveManager>();
        }
        
        private void SetCurrentLevelData(string levelName)
        {
            currentLevelData = Resources.Load<LevelData>($"ScriptableObjects/Levels/LD_{levelName}");
            Debug.Log($"ScriptableObjects/Levels/LD_{levelName}");
        }
        
        private void OnLevelLoaded()
        {
            /*var spawnPoint = GameObject.FindWithTag("Respawn");
            player = Instantiate(GameManager.Instance.GetPlayerPrefab, spawnPoint.transform.position, Quaternion.identity);*/
            isLevelLoaded = true;
            Debug.Log($"Level loaded: {currentLevelData.LevelName}");
            Debug.Log($"Current level: {currentLevelData.LevelName} - {currentLevelData.LevelNumber} - IsBossLevel: {currentLevelData.IsBossLevel} - IsWaveLevel: {currentLevelData.IsWaveLevel}");

            if (currentLevelData.IsBossLevel)
            {
                Debug.Log("Boss level");
            }
            else if (currentLevelData.IsWaveLevel)
            {
                Debug.Log("Wave level");
                waveManager.enabled = true;
            }
            else
            {
                Debug.Log("Normal level");
            }
            
            player = GameObject.FindWithTag("Player");
            GameManager.Instance.OnLevelLoaded();
            ObjectPoolManager.Instance.Initialize();
        }
        
        private void OnLevelUnloaded()
        {
            Debug.Log($"Level unloaded: {currentLevelData.LevelName}");
            currentLevelData = null;
            ObjectPoolManager.Instance.ClearPooledObjects();
            GameManager.Instance.OnLevelUnloaded();
        }
        
        private IEnumerator LoadLevelCoroutine(string levelName)
        {
            SetCurrentLevelData(levelName);
            if (currentLevelData == null)
            {
                Debug.LogError("Current level data is null");
                yield break;
            }
            
            isLevelLoaded = false;
            levelLoadingOperation = SceneManager.LoadSceneAsync(currentLevelData.LevelName);
            while (!levelLoadingOperation.isDone)
            {
                yield return null;
            }
            
            OnLevelLoaded();
        }
        
        public void LoadLevel(string levelName)
        {
            if (isLevelLoaded)
            {
                StartCoroutine(UnloadLevelCoroutine());
            }
            
            StartCoroutine(LoadLevelCoroutine(levelName));
        }
        
        private IEnumerator UnloadLevelCoroutine()
        {
            isLevelLoaded = false;
            levelLoadingOperation = SceneManager.UnloadSceneAsync(currentLevelData.LevelName);
            while (!levelLoadingOperation.isDone)
            {
                yield return null;
            }

            OnLevelUnloaded();
        }

        public void WinLevel()
        {
            Debug.Log("Win level");
        }
        
        public void FailLevel()
        {
            Debug.Log("Fail level");
            failedLevelCount++;
        }

        public void Reset()
        {
            failedLevelCount = 0; 
        }
    }
}