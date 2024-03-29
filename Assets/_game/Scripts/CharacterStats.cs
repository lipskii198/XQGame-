﻿using System;
using UnityEngine;

namespace _game.Scripts
{
    [Serializable]
    public class CharacterStats
    {
        [SerializeField] private bool allowStatsCalculation = true;

        [Header("Character Stats (Overall)")] 
        public float movementSpeed;
        public float jumpSpeed;
        public float attackSpeed;
        public float health;

        [Header("Character Stats (Base)")] 
        [SerializeField] private float baseMovementSpeed;
        [SerializeField] private float baseJumpForce;
        [SerializeField] private float baseAttackSpeed;
        [SerializeField] private float baseHealth;

        public void CalculateOverallStats()
        {
            if(!allowStatsCalculation) return;
            movementSpeed = baseMovementSpeed;
            jumpSpeed = baseJumpForce;
            attackSpeed = baseAttackSpeed;
            health = baseHealth;
            allowStatsCalculation = false;
        }
    }
}