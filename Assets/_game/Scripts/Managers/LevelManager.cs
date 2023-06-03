using System.Collections;
using _game.Scripts.ObjectPooling;
using _game.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _game.Scripts.Managers
{
    public class LevelManager : MonoBehaviour
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
        public bool IsLevelLoaded => isLevelLoaded;
        

        private void SetCurrentLevelData(string levelName)
        {
            currentLevelData = Resources.Load<LevelData>($"ScriptableObjects/Levels/LD_{levelName}");
        }

        private void OnLevelLoaded()
        {
            /*var spawnPoint = GameObject.FindWithTag("Respawn");
            player = Instantiate(GameManager.Instance.GetPlayerPrefab, spawnPoint.transform.position, Quaternion.identity);*/
            
            Debug.Log($"Level Loaded: LevelName#Id: {currentLevelData.LevelName}#{currentLevelData.Id} - IsBossLevel: {currentLevelData.IsBossLevel} - IsWaveLevel: {currentLevelData.IsWaveLevel}");
            
            player = GameObject.FindWithTag("Player");
            
            GameManager.Instance.OnLevelLoaded();
            isLevelLoaded = true;

            if (currentLevelData.IsBossLevel)
            {
                Debug.Log("Boss level");
            }
            else if (currentLevelData.IsWaveLevel)
            {
                Debug.Log("Wave level");
                GameManager.Instance.GetWaveManager.enabled = true;
            }
            else
            {
                Debug.Log("Normal level");
            }


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
               UnloadLevel();
            }

            StartCoroutine(LoadLevelCoroutine(levelName));
        }
        
        public void UnloadLevel(bool mainMenu = false)
        {
            if (!isLevelLoaded) return;
            isLevelLoaded = false;
            levelLoadingOperation = SceneManager.UnloadSceneAsync(currentLevelData.LevelName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            OnLevelUnloaded();
            
            if (mainMenu)
            {
                SceneManager.LoadSceneAsync("MainMenu");
            }
            
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