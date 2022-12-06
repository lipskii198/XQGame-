using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public void OnPlayerDeath()
    {
        Debug.Log("Player died");
    }
}