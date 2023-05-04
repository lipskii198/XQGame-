using _game.Scripts.Managers;
using _game.Scripts.Player;
using _game.Scripts.ScriptableObjects;
using UnityEngine;

namespace _game.Scripts.Enemies.Core
{
    public abstract class EnemyBossBase : MonoBehaviour //TODO: Inheriting from EnemyBase is too much of headache but refactor later
    {
        [SerializeField] protected int currentPhase = 1;
        [SerializeField] protected float currentHealth;
        [SerializeField] protected float timeSinceLastAttack;
        [SerializeField] protected bool isFollowingPlayer;
        [SerializeField] protected EnemyBossData bossData;

        
        protected Transform playerTransform;
        protected Animator animator;
        protected Rigidbody2D rb;
        
        protected virtual void Awake()
        {
            playerTransform = GameObject.FindWithTag("Player").transform;
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            currentPhase = 1;
            currentHealth = GetCurrentHealthThreshold();
        }

        protected void Update()
        {
            if (currentHealth <= 0)
            {
                Die();
                return;
            }

            if (currentPhase >= bossData.PhasesAmount)
            {
                Debug.Log("42069");
                Die();
                return;
            }

            if (currentHealth <= GetCurrentHealthThreshold())
            {
                currentPhase++;
            }

            if (GetDistanceToPlayer() <= GetCurrentAttackRange())
            {
                if (timeSinceLastAttack >= GetCurrentAttackCooldown())
                {
                    Attack();
                    timeSinceLastAttack = 0f;
                    isFollowingPlayer = false;
                }
                else
                {
                    timeSinceLastAttack += Time.deltaTime;
                    isFollowingPlayer = true;
                }
            }
            else
            {
                if (!isFollowingPlayer)
                {
                    Patrol(); // Do we want the boss to patrol?
                }
                else
                {
                    //ChasePlayer(AiTarget.Player);
                }
            }
        }


        protected virtual void Attack()
        {
            animator.SetTrigger("Attack");
            playerTransform.GetComponent<PlayerManager>().TakeDamage(GetCurrentDamage());
            animator.SetBool("IsMoving", false);
        }

        protected virtual void Patrol()
        {
            //TODO: Implement patrolling behavior for bosses
        }

        protected virtual void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentPhase < bossData.PhasesAmount - 1 && currentHealth <= GetCurrentHealthThreshold())
            {
                currentPhase++;
                currentHealth = GetCurrentHealthThreshold();
                animator.SetTrigger("PhaseTransition");
            }
            else
            {
                animator.SetTrigger("Hurt");
            }
        }

        protected virtual void Die()
        {
            animator.SetBool("IsDead", true);
            Destroy(gameObject, 2f);
        }
        
        
        protected float GetDistanceToPlayer()
        {
            var playerPosition = playerTransform.position;
            return Vector2.Distance(playerPosition, transform.position);
        }
        
        
        protected int GetCurrentHealthThreshold()
        {
            return bossData.PhasesHealthThreshold[currentPhase - 1];
        }

        protected int GetCurrentMovementSpeed()
        {
            return bossData.PhasesMovementSpeed[currentPhase - 1];
        }

        protected float GetCurrentAttackCooldown()
        {
            return bossData.PhasesAttackCooldown[currentPhase - 1];
        }

        protected int GetCurrentAttackRange()
        {
            return bossData.PhasesAttackRange[currentPhase - 1];
        }

        protected int GetCurrentDamage()
        {
            return bossData.PhasesDamage[currentPhase - 1];
        }
        
    }
}