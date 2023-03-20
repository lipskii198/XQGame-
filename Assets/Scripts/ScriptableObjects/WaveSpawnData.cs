using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "WaveSpawnData", menuName = "ScriptableObjects/WaveSpawnData", order = 0)]
    public class WaveSpawnData : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private List<int> enemiesPerWave;
        [SerializeField] private int spawnDelay;
        [SerializeField] private int maxWaves;
        [SerializeField] private List<GameObject> enemiesPrefabs;


        public int Id => id;
        public List<int> EnemiesPerWave => enemiesPerWave;
        public float SpawnDelay => spawnDelay;
        public int MaxWaves => maxWaves;
        public List<GameObject> EnemiesPrefabs => enemiesPrefabs;
    }
}