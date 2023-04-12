using System;
using System.Collections;
using _game.Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _game.Scripts.Enemies.Core
{
    public abstract class EnemyBase : MonoBehaviour
    {
        [SerializeField] protected EnemyData enemyData;
        [SerializeField] protected float currentHealth;
        [SerializeField] protected float timeSinceLastAttack;
        [SerializeField] protected float deathFadeTime = 1.5f;
        [SerializeField] protected bool isFollowingPlayer;
        [SerializeField] protected bool canMove;
        
        protected bool isFacingRight = true;
        protected Transform playerTransform;
        protected Transform parentHolder;
        protected Animator animator;
        protected Rigidbody2D rb;
        private Camera mainCamera;

        
        public UnityEvent onDeath = new();
        protected virtual void Awake()
        {
            mainCamera = Camera.main;
            playerTransform = GameObject.FindWithTag("Player").transform;
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            currentHealth = enemyData.Health;

            parentHolder = transform.parent;
            
            IgnoreCollisionWithPlayer();

        }

        private void IgnoreCollisionWithPlayer()
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerTransform.GetComponent<Collider2D>());
        }
        

        protected virtual void Update()
        {
            if (currentHealth <= 0)
            {
                Die();
                return;
            }
            
        }

        protected virtual void FixedUpdate()
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

        
        //TODO: Disable movement when hit / in hurt animation
        public virtual void TakeDamage(float damage)
        {
            currentHealth -= damage;
            animator.SetTrigger("Hurt");
            isFollowingPlayer = true;
        }

        protected virtual void Die()
        {
            onDeath.Invoke();
            animator.SetTrigger("Death");
            StartCoroutine(DeathFade());
            this.enabled = false;
        }

        protected virtual void FaceDirection(Vector2 direction)
        {
            if (direction.x > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (direction.x < 0 && isFacingRight)
            {
                Flip();
            }
        }
        
        protected void Flip()
        {
            isFacingRight = !isFacingRight;
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        
        protected float GetDistanceToPlayer()
        {
            var playerPosition = playerTransform.position;
            return Vector2.Distance(playerPosition, transform.position);
        }
        
        protected IEnumerator DeathFade()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            var color = spriteRenderer.color;
            
            yield return new WaitForSeconds(deathFadeTime);
            
            while (color.a > 0)
            {
                color.a -= Time.deltaTime;
                spriteRenderer.color = color;
                yield return null;
            }
            Destroy(parentHolder.gameObject);
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