using _game.Scripts.Enemies.Core;
using _game.Scripts.ScriptableObjects;
using UnityEngine;

namespace _game.Scripts.Enemies.NormalEnemies
{
    public class AntEnemy : EnemyBase<EnemyData>
    {
        protected override void Awake()
        {
            base.Awake();
            currentHealth = enemyData.Health;
            parentHolder = transform.parent;  
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