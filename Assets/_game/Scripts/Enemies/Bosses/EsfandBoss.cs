using System.Collections;
using _game.Scripts.Enemies.Core;
using _game.Scripts.Player;
using _game.Scripts.Utility;
using UnityEngine;

namespace _game.Scripts.Enemies.Bosses
{
    public class EsfandBoss : EnemyBossBase
    {
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float jumpCooldown = 3f;
        [SerializeField] private float landCheckDelay = 0.5f;
        [SerializeField] private float landingRadius = 3f;
        [SerializeField] private float landDamageMultiplier = 1.5f;

        [SerializeField] private bool hasLanded;
        [SerializeField] private bool isJumpAttack;
        [SerializeField] private bool isAttackBlocked;
        [SerializeField] private bool isJumpBlocked;

        // Block lists
        public Blocklist JumpBlocklist { get; } = new();

        public Blocklist AttackBlocklist { get; } = new();

        // Block lists objects
        private object jumpBlocker;
        private object attackBlocker;

        protected override void Awake()
        {
            base.Awake();
            isFollowingPlayer = true;
            isFacingRight = false;
            currentPhase = 1;
        }


        protected override void Update()
        {
            base.Update();

            isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);
            isAttackBlocked = AttackBlocklist.IsBlocked();
            isJumpBlocked = JumpBlocklist.IsBlocked();

            if (!isFightTriggered || currentPhase > enemyData.PhasesAmount) return;

            if (!hasLanded && isJumpAttack && isGrounded && rb.velocity.y == 0)
            {
                hasLanded = true;
                StartCoroutine(StartOnLand());
            }
        }

        protected override void FixedUpdate()
        {
            if (!isFightTriggered || currentPhase > enemyData.PhasesAmount) return;

            if (GetDistanceToPlayer() <= GetCurrentAttackRange())
            {
                if (timeSinceLastAttack >= GetCurrentAttackCooldown() && !AttackBlocklist.IsBlocked())
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
                if (isFollowingPlayer && !JumpBlocklist.IsBlocked() && !isJumpAttack)
                {
                    ChasePlayer();
                }
            }
        }

        protected override void ChasePlayer() // Jump after player
        {
            if (!isGrounded) return;
            AttackBlocklist.RegisterBlocker(jumpBlocker);
            FacePlayer();
            isJumpAttack = true;
            rb.AddForce(new Vector2(playerTransform.position.x - transform.position.x, jumpForce), ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
            StartCoroutine(StartJumpCooldown());
        }

        protected override void Attack()
        {
            JumpBlocklist.RegisterBlocker(attackBlocker);
            FacePlayer();
            animator.SetTrigger("Attack");
            var knockBackDirection = playerTransform.position - transform.position;
            playerTransform.GetComponent<PlayerManager>()
                .TakeDamage(GetCurrentDamage(), knockBack: true, knockBackDirection);

            JumpBlocklist.UnregisterBlocker(attackBlocker);
        }

        private IEnumerator StartJumpCooldown()
        {
            JumpBlocklist.RegisterBlocker(jumpBlocker);
            yield return new WaitForSeconds(jumpCooldown);
            JumpBlocklist.UnregisterBlocker(jumpBlocker);
        }

        private IEnumerator StartOnLand()
        {
            Debug.Log("Landed");
            yield return new WaitForSeconds(landCheckDelay);
            animator.SetBool("IsJumping", false);
            hasLanded = false;
            isJumpAttack = false;
            AttackBlocklist.UnregisterBlocker(jumpBlocker);

            if (!IsPlayerInLandingRadius()) yield break;

            var knockBackDirection = playerTransform.position - transform.position;
            playerTransform.GetComponent<PlayerManager>().TakeDamage(GetCurrentDamage() * landDamageMultiplier,
                knockBack: true, knockBackDirection);
        }

        private bool IsPlayerInLandingRadius()
        {
            return Vector2.Distance(transform.position, playerTransform.position) <= landingRadius;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, landingRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, GetCurrentAttackRange());
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
        }
    }
}