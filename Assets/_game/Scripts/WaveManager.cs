using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _game.Scripts.Enemies.Core;
using _game.Scripts.Managers;
using _game.Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace _game.Scripts
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private int currentWave;
        [SerializeField] private int currentEnemyCount;
        [SerializeField] private float textDisplayTime;
        [SerializeField] private bool isWaveSpawning;
        [SerializeField] private WaveSpawnData waveSpawnData;
        [SerializeField] private GameObject waveHUDPrefab;
        [SerializeField] private Transform parentHolder;
        [SerializeField] private List<Transform> spawnPoints;
    
        private LevelManager levelManager;
        private TMP_Text waveCounterText;
        private TMP_Text newWaveText;
        private static Transform SpawnPointParent => GameObject.Find("SpawnPoints").transform;
        private void Awake()
        {
            isWaveSpawning = false;
            spawnPoints = new List<Transform>();
        }

        private void Start()
        {
            levelManager = GetComponent<LevelManager>();

            waveSpawnData = Resources.Load<WaveSpawnData>($"ScriptableObjects/Waves/WD_{levelManager.GetCurrentLevelData.LevelName}");

            if (waveSpawnData == null || waveSpawnData.EnemiesPrefabs.Count == 0  || !EnsureEnemyPrefabIsValid()){
                Debug.LogError($"WaveSpawnData ({waveSpawnData.name}) is invalid");
                this.enabled = false;
            }

            if (SpawnPointParent == null || SpawnPointParent.childCount == 0)
            {
                Debug.LogError($"SpawnPointParent ({levelManager.GetCurrentLevelData.LevelName}) is invalid");
                this.enabled = false;
            }
        
            for (var i = 0; i < SpawnPointParent.childCount; i++)
            {
                spawnPoints.Add(SpawnPointParent.GetChild(i));
            }
        
            parentHolder = GameObject.Find("UICanvas").transform;

            var waveHud = Instantiate(waveHUDPrefab, parentHolder);
        
            waveCounterText = waveHud.transform.Find("WaveCounterText").GetComponent<TMP_Text>();
            newWaveText = waveHud.transform.Find("NewWaveText").GetComponent<TMP_Text>();

        }

        private void Update()
        {
            if (!isWaveSpawning && currentWave < waveSpawnData.MaxWaves)
                StartCoroutine(BeginWaveSpawn());
        
            if (currentWave >= waveSpawnData.MaxWaves)
                levelManager.WinLevel();
        }
    
        private IEnumerator BeginWaveSpawn()
        {
            SpawnWave();
            currentWave++;
            waveCounterText.text = $"Wave {currentWave} / {waveSpawnData.MaxWaves}";
            isWaveSpawning = true;
            yield return new WaitForSeconds(waveSpawnData.SpawnDelay);
            isWaveSpawning = false;
        }
    
        private void SpawnWave()
        {
            StartCoroutine(DisplayNewWaveText());
            for (var i = 0; i < waveSpawnData.EnemiesPerWave.Count; i++)
            {
                for (var j = 0; j < waveSpawnData.EnemiesPerWave[i]; j++)
                {
                    var spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
                    var enemy = Instantiate(waveSpawnData.EnemiesPrefabs[i], spawnPoints[spawnPointIndex].position, Quaternion.identity);
                    enemy.GetComponentInChildren<EnemyBase>().onDeath.AddListener(OnEnemyDeath);
                    currentEnemyCount++;
                }
            }
        }
    
        private void OnEnemyDeath()
        {
            currentEnemyCount--;
        
            // TODO: check if wave is spawning before spawning new wave
            /*if (currentEnemyCount <= 0)
            StartCoroutine(BeginWaveSpawn());*/
        
        }
    
        private bool EnsureEnemyPrefabIsValid()
        {
            return waveSpawnData.EnemiesPrefabs.All(enemy => enemy != null);
        }
    
        private IEnumerator DisplayNewWaveText()
        {
            newWaveText.enabled = true;
            yield return new WaitForSeconds(textDisplayTime);
            newWaveText.enabled = false;
        }
    }
}