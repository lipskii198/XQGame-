using _game.Scripts.Enemies.Core;
using UnityEngine;

namespace _game.Scripts.Enemies
{
    public class AntEnemy : EnemyBase
    {
        protected override void Awake()
        {
            base.Awake();
            isFollowingPlayer = true;
            isFacingRight = true;
        }

        protected override void FixedUpdate()
        {
            ChasePlayer();
        }

        protected override void ChasePlayer()
        {
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
    }
}