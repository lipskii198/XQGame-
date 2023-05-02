using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Managers;

namespace Enemies
{
    public class BossController : MonoBehaviour
    {
        [SerializeField] private float currentHealth;
        [SerializeField] private float bossMovementSpeed;
        [SerializeField] private float attackRange;
        
        
        [SerializeField] private BossState currentState;
        [SerializeField] private bool isFightTriggered;
        [SerializeField] private Image bossHealthBar;
        [SerializeField] private GameObject fightTrigger;

        [Header("attack")]
        [SerializeField] private float attackCooldown;
        [SerializeField] float jumpHeight;
        [SerializeField] float damage;

        
        
        [SerializeField] Transform player;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] LayerMask playerLayer;
        [SerializeField] Transform groundCheck;
        [SerializeField] Transform boss;
        [SerializeField] Vector2 boxSize;
        [SerializeField] Vector2 hitSize;
        [SerializeField] Vector2 knockback;
        [SerializeField] BoxCollider2D bossCollider;
        [SerializeField] Rigidbody2D playerRB;

        private Rigidbody2D bossRB;
        private bool isGrounded;
        private bool canAttack;
        private float time;
        private float startY;


        [Header("Debug")] [SerializeField] private TMP_Text stateText;

        private GameObject playerObject;
        private void Start()
        {
            bossRB = GetComponent<Rigidbody2D>();
            currentState = BossState.Idle;
            playerObject = GameObject.FindWithTag("Player");
            time = 0;
            startY = boss.position.y;
        }

        private void Update()
        {
            time += Time.deltaTime;
            isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, groundLayer);
            canAttack = Physics2D.OverlapBox(groundCheck.position, hitSize, 0, playerLayer);

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
            if (time > attackCooldown && isGrounded)
            {
                time = 0;
                JumpAttack();

            }
            if( canAttack && landing())
            {
                StartCoroutine(AllowEscape());
                player.GetComponent<PlayerManager>().TakeDamage(damage);
                playerRB.AddForce(knockback, ForceMode2D.Impulse);
                bossRB.AddForce(new Vector2(0, 3), ForceMode2D.Impulse); ;
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
                bossRB.velocity = Vector2.zero;
                currentState = BossState.Attacking;
            }
            else
            {
                bossRB.velocity = (playerPosition - bossPosition).normalized * bossMovementSpeed;
            }
        }

        private void OnAttackCheck()
        {
            StartCoroutine(Attack());
            if (GetDistanceToPlayer() > attackRange) currentState = BossState.Moving;
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
        private float GetDistanceToPlayer()
        {
            return player.position.x - boss.position.x;
        }
        private void JumpAttack()
        {
            bossRB.AddForce(new Vector2(GetDistanceToPlayer(), jumpHeight), ForceMode2D.Impulse);
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(groundCheck.position, boxSize);
            Gizmos.color = Color.red;
            Gizmos.DrawCube(groundCheck.position, hitSize);
        }
        private bool landing()
        {
            if (boss.position.y < startY + 2 && bossRB.velocity.y < 0)
                return true;
            return false;
        }
        private IEnumerator AllowEscape()
        {
            bossCollider.enabled = false;
            yield return new WaitForSeconds(1);
            bossCollider.enabled = true;
        }
    }
}