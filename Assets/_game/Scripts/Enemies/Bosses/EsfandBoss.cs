using System;
using System.Collections;
using _game.Scripts.Enemies.Core;
using _game.Scripts.Player;
using Unity.Mathematics;
using UnityEngine;

namespace _game.Scripts.Enemies.Bosses
{
    public class EsfandBoss : EnemyBossBase
    {
        
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float jumpCooldown = 3f;
        [SerializeField] private float landingRadius = 3f;
        
        private bool isLanding;
        protected override void Awake()
        {
            base.Awake();
            isFollowingPlayer = true;
            isFacingRight = false;
        }


        protected override void Update()
        {
            base.Update();

            if (!isLanding && isGrounded && rb.velocity.y == 0)
            {
                isLanding = true;
                StartCoroutine(BeginLandingDelay());
            }
            
            if (GetDistanceToPlayer() <= GetCurrentAttackRange() && isGrounded)
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
                if (isFollowingPlayer)
                {
                    ChasePlayer();
                }
            }
        }

        protected override void ChasePlayer() // Jump after player
        {
            if(!isGrounded) OnLand();
            rb.AddForce(new Vector2(playerTransform.position.x - transform.position.x, jumpForce), ForceMode2D.Impulse);
            var direction = playerTransform.position - transform.position;
            FaceDirection(direction);
            StartCoroutine(BeginJumpCooldown());
        }

        protected override void Attack()
        {
            playerTransform.GetComponent<PlayerManager>().SetTarget(gameObject);
            var knockBackDirection = playerTransform.position - transform.position;
            playerTransform.GetComponent<PlayerManager>().TakeDamage(0, knockBack: true, knockBackDirection);
        }

        private IEnumerator BeginJumpCooldown()
        {
            isFollowingPlayer = false;
            yield return new WaitForSeconds(jumpCooldown);
            isFollowingPlayer = true;
        }
        
        private IEnumerator BeginLandingDelay()
        {
            OnLand();
            yield return new WaitForSeconds(jumpCooldown);
            isLanding = false;
        }
        
        private bool IsPlayerInLandingRadius()
        {
            return Vector2.Distance(transform.position, playerTransform.position) <= landingRadius;
        }
        
        private void OnLand()
        {
            if (IsPlayerInLandingRadius())
            {
                Attack();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, landingRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
        }
    }
}