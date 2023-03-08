using Managers;
using ScriptableObjects;
using UnityEngine;

namespace Enemies.Core
{
    public abstract class EnemyBase : MonoBehaviour
    {
        [SerializeField] private EnemyData enemyData;
        [SerializeField] protected float currentHealth;
        [SerializeField] protected float timeSinceLastAttack;
        [SerializeField] protected bool isFollowingPlayer;
        
        protected Transform playerTransform;
        protected Animator animator;
        protected Rigidbody2D rb;

        protected virtual void Awake()
        {
            playerTransform = GameObject.FindWithTag("Player").transform;
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            currentHealth = enemyData.Health;
        }


        protected virtual void Update()
        {
            if (currentHealth <= 0)
            {
                Die();
                return;
            }

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
                    timeSinceLastAttack = Time.deltaTime;
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
                    Goto(); // Goto(AiTarget.Player);
                }
            }
            
        }

        protected virtual void Patrol()
        {
            // Override me
        }

        protected virtual void Goto()
        {
            //TODO
        }

        protected virtual void Attack()
        {
            animator.SetTrigger("Attack");
            playerTransform.GetComponent<PlayerManager>().TakeDamage(enemyData.Damage);
            animator.SetBool("IsMoving", false);
        }

        public virtual void TakeDamage(float damage)
        {
            currentHealth -= damage;
            animator.SetTrigger("Hurt");
        }

        protected virtual void Die()
        {
            animator.SetBool("IsDead", true);
            Destroy(gameObject, 1f);
        }
        
        protected float GetDistanceToPlayer()
        {
            var playerPosition = playerTransform.position;
            return Vector2.Distance(playerPosition, transform.position);
        }
    }
}