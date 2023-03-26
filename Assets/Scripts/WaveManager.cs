using System;
using System.Collections;
using System.Collections.Generic;
using Enemies.Core;
using ScriptableObjects;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private int currentWave;
    [SerializeField] private bool isWaveSpawning;
    [SerializeField] private WaveSpawnData waveSpawnData;
    
    [SerializeField] private List<Transform> spawnPoints;
    private static Transform SpawnPointParent => GameObject.Find("SpawnPoints").transform;
    private void Awake()
    {
        isWaveSpawning = false;
        spawnPoints = new List<Transform>();
    }

    private void Start()
    {
        waveSpawnData = Resources.Load<WaveSpawnData>($"ScriptableObjects/Waves/WD_{GameManager.Instance.GetCurrentLevelData.LevelName}");
        
        for (var i = 0; i < SpawnPointParent.childCount; i++)
        {
            spawnPoints.Add(SpawnPointParent.GetChild(i));
        }
        
        if (waveSpawnData == null)
            Debug.LogError("WaveSpawnData is null");
        
        if (SpawnPointParent == null)
            Debug.LogError("SpawnPointParent is null");
    }

    private void Update()
    {
        if (!isWaveSpawning && currentWave < waveSpawnData.MaxWaves)
            StartCoroutine(BeginWaveSpawn());
    }
    
    private IEnumerator BeginWaveSpawn()
    {
        SpawnWave();
        currentWave++;
        isWaveSpawning = true;
        yield return new WaitForSeconds(waveSpawnData.SpawnDelay);
        isWaveSpawning = false;
    }
    
    private void SpawnWave()
    {
        for (var i = 0; i < waveSpawnData.EnemiesPerWave.Count; i++)
        {
            for (var j = 0; j < waveSpawnData.EnemiesPerWave[i]; j++)
            {
                var spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
                var enemy = Instantiate(waveSpawnData.EnemiesPrefabs[i], spawnPoints[spawnPointIndex].position, Quaternion.identity);
            }
        }
    }
}