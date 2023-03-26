using ScriptableObjects;
using UnityEngine;

public class GameManager : LazySingletonMono<GameManager>
{
    [SerializeField] private LevelData currentLevelData;


    private WaveManager waveManager;
    public LevelData GetCurrentLevelData => currentLevelData;
    
    private void Start()
    {
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
    
    public void OnPlayerDeath()
    {
        Debug.Log("Player died");
    }
}