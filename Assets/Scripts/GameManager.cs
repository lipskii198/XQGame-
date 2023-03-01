using System;
using UnityEngine;

public class GameManager : LazySingletonMono<GameManager>
{
    public void OnPlayerDeath()
    {
        Debug.Log("Player died");
    }
}