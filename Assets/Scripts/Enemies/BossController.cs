using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies
{
    public class BossController : MonoBehaviour
    {
        [SerializeField] private float currentHealth;
        [SerializeField] private float bossMovementSpeed;
        [SerializeField] private float attackRange;
        [SerializeField] private float attackCooldown;
        
        [SerializeField] private BossState currentState;
        [SerializeField] private bool isFightTriggered;
        [SerializeField] private Image bossHealthBar;
        [SerializeField] private GameObject fightTrigger;


        [Header("Debug")] [SerializeField] private TMP_Text stateText;

        private GameObject playerObject;
        private new Rigidbody2D rigidbody;
        private void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            currentState = BossState.Idle;
            playerObject = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            if (currentHealth <= 0)
            {
                Debug.Log("Boss: *Dies* ");
                enabled = false;
            }

            switch (currentState)
            {
                case BossState.Idle:
                    OnIdleCheck();
                    break;
                case BossState.Moving:
                    OnMoveCheck();
                    break;
                case BossState.Attacking:
                    OnAttackCheck();
                    break;
            }
            
            stateText.text = $"BossState: {currentState}\n isFightTriggered: {isFightTriggered}";
            bossHealthBar.fillAmount = currentHealth / 100;
        }

        private void OnIdleCheck()
        {
            if (isFightTriggered)
            {
                fightTrigger.SetActive(false);
                currentState = BossState.Moving;
            }
        }

        private void OnMoveCheck()
        {
            var playerPosition = playerObject.transform.position;
            var bossPosition = transform.position;
            
            if (GetDistanceToPlayer() <= attackRange)
            {
                rigidbody.velocity = Vector2.zero;
                currentState = BossState.Attacking;
            }
            else
            {
                rigidbody.velocity = (playerPosition - bossPosition).normalized * bossMovementSpeed;
            }
        }

        private void OnAttackCheck()
        {
            StartCoroutine(Attack());
            if (GetDistanceToPlayer() > attackRange) currentState = BossState.Moving;
        }


        private float GetDistanceToPlayer()
        {
            var playerPosition = playerObject.transform.position;
            var distanceToPlayer = Vector2.Distance(playerPosition, transform.position);
            return distanceToPlayer;
        }

        private IEnumerator Attack()
        {
            // Play animation
            // Damage the player if nearby
            currentState = BossState.Cooldown;
            yield return new WaitForSeconds(attackCooldown);
            currentState = BossState.Moving;
        }
        
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (isFightTriggered) return;

            isFightTriggered = col.CompareTag("Player");
        }
        
        
        private enum BossState
        {
            Idle, // Fight didnt begin
            Moving, // Following the player
            Attacking, // Basic attack?
            Cooldown, // Rest after attacking?
        }

    }
}