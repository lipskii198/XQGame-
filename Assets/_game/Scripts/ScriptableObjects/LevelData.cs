using UnityEngine;

namespace _game.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "LevelData", order = 0)]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private string levelName;
        [SerializeField] private int levelNumber;
        [SerializeField] private bool isBossLevel;
        [SerializeField] private bool isWaveLevel;
        
        
        public int Id => id;
        public string LevelName => levelName;
        public int LevelNumber => levelNumber;
        public bool IsBossLevel => isBossLevel;
        public bool IsWaveLevel => isWaveLevel;
    }
}