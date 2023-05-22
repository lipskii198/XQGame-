using _game.Scripts.Managers;
using _game.Scripts.Player;
using _game.Scripts.ScriptableObjects;
using UnityEngine;

namespace _game.Scripts.Enemies.Core
{
    public abstract class EnemyBossBase : EnemyBase<EnemyBossData>
    {
        [SerializeField] protected int currentPhase = 0;
        [SerializeField] protected bool isFightTriggered = false;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);


            if (currentHealth <= 0)
            {
                Debug.Log("oh no im dead");
                //Die();
                return;
            }

            if (currentPhase > enemyData.PhasesAmount)
            {
                Debug.Log("42069");
                //Die();
                return;
            }

            if (currentHealth <= GetCurrentHealthThreshold())
            {
                currentPhase++;
            }
        }

        protected override void FixedUpdate()
        {
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
                    Idle();
                }
                else
                {
                    ChasePlayer();
                }
            }
        }


        protected override void Attack()
        {
            animator.SetTrigger("Attack");
            playerTransform.GetComponent<PlayerManager>().TakeDamage(GetCurrentDamage());
            animator.SetBool("IsMoving", false);
        }

        protected override void Patrol()
        {
            // Boss doesn't patrol
        }

        public override void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentPhase < enemyData.PhasesAmount - 1 && currentHealth <= GetCurrentHealthThreshold())
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


        protected int GetCurrentHealthThreshold() => enemyData.PhasesHealthThreshold[currentPhase - 1];

        protected int GetCurrentMovementSpeed() => enemyData.PhasesMovementSpeed[currentPhase - 1];

        protected float GetCurrentAttackCooldown() => enemyData.PhasesAttackCooldown[currentPhase - 1];

        protected int GetCurrentAttackRange() => enemyData.PhasesAttackRange[currentPhase - 1];

        protected int GetCurrentDamage() => enemyData.PhasesDamage[currentPhase - 1];
    }
}