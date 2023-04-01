using System;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Enemies.Core
{
    public abstract class EnemyBase : MonoBehaviour
    {
        [SerializeField] protected EnemyData enemyData;
        [SerializeField] protected float timeSinceLastAttack;
        [SerializeField] protected bool isFollowingPlayer;
        
        [Header("Health Info")]
        [SerializeField] protected float currentHealth;
        [SerializeField] protected GameObject healthBar;
        [SerializeField] protected Image healthBarForeground; 
        [SerializeField] protected TMP_Text healthBarText;
        
        protected Transform playerTransform;
        protected Transform parentHolder;
        protected Animator animator;
        protected Rigidbody2D rb;
        private UnityEngine.Camera mainCamera;
        
        
        public UnityEvent onDeath = new();
        protected virtual void Awake()
        {
            mainCamera = UnityEngine.Camera.main;
            playerTransform = GameObject.FindWithTag("Player").transform;
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            currentHealth = enemyData.Health;

            parentHolder = transform.parent;
        }


        protected virtual void Update()
        {
            healthBar.transform.position = mainCamera.WorldToScreenPoint(transform.position + new Vector3(0, 2f, 0));
            healthBarForeground.fillAmount = currentHealth / enemyData.Health;
            healthBarText.text = $"{currentHealth} / {enemyData.Health}";
            
            if (currentHealth <= 0)
            {
                Die();
                return;
            }

        }

        private void FixedUpdate()
        {
            if (GetDistanceToPlayer() <= enemyData.AttackRange)
            {
                if (timeSinceLastAttack >= enemyData.AttackSpeed)
                {
                    Attack();
                    timeSinceLastAttack = 0;
                    isFollowingPlayer = false;
                }
                else
                {
                    timeSinceLastAttack += Time.deltaTime;
                    isFollowingPlayer = transform;
                }
            }
            else
            {
                if (!isFollowingPlayer)
                {
                    Patrol();
                }
                else
                {
                    ChasePlayer();
                }
            }
        }

        protected virtual void Patrol()
        {
            animator.SetBool("IsMoving", true);
            //TODO: Implement patrol
        }
        
        protected virtual void ChasePlayer()
        {
            animator.SetBool("IsMoving", true);
            var playerPosition = playerTransform.position;
            var position = transform.position;

            if (GetDistanceToPlayer() <= enemyData.AttackRange)
            {
                rb.velocity = Vector2.zero;
            }
            else
            {
                rb.velocity = (playerPosition - position).normalized * enemyData.MovementSpeed;
                var direction = playerPosition - position;
                FaceDirection(direction);
            }
        }
        
        protected virtual void Idle()
        {
            animator.SetBool("IsMoving", false);
        }
        
        protected virtual void Attack()
        {
            animator.SetBool("IsMoving", false);
            animator.SetTrigger("Attack");
        }

        public virtual void TakeDamage(float damage)
        {
            currentHealth -= damage;
            animator.SetTrigger("Hurt");
        }

        protected virtual void Die()
        {
            onDeath.Invoke();
            animator.SetTrigger("Death");
            Destroy(parentHolder.gameObject, 2f);
            this.enabled = false;
            healthBar.SetActive(false);
        }
        
        protected virtual void FaceDirection(Vector2 direction)
        {
            transform.localScale = direction.x switch
            {
                < 0 => new Vector3(1, 1, 1),
                > 0 => new Vector3(-1, 1, 1),
                _ => transform.localScale
            };
        }
        
        protected float GetDistanceToPlayer()
        {
            var playerPosition = playerTransform.position;
            return Vector2.Distance(playerPosition, transform.position);
        }

        protected void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                rb.mass = 9999;
            }
        }
        
        protected void OnCollisionExit2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                rb.mass = 1;
            }
        }
    }
}