using System.Collections.Generic;
using _game.Scripts.Enemies.Core;
using _game.Scripts.ObjectPooling;
using _game.Scripts.ScriptableObjects;
using UnityEngine;

namespace _game.Scripts.Enemies.NormalEnemies
{
    public class BananaEnemy : EnemyBase<EnemyData>
    {
        [SerializeField] private ProjectileData projectileData;
        [SerializeField] private Transform patrolPointHolder;
        [SerializeField] private List<Transform> patrolPoints;
        [SerializeField] private Transform targetPatrolPoint;
        [SerializeField] private int currentPatrolIndex;
        [SerializeField] private Transform firePoint;

        protected override void Awake()
        {
            base.Awake();
            currentHealth = enemyData.Health;
            parentHolder = transform.parent;  
        }

        private void Start()
        {
            foreach (var patrolPoint in patrolPointHolder.GetComponentsInChildren<Transform>())
            {
                if (patrolPoint == patrolPointHolder.transform) continue;
                patrolPoints.Add(patrolPoint);
            }
            isFacingRight = false;
        }

        protected override void Patrol()
        {
            base.Patrol();
            if (targetPatrolPoint == null)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
                targetPatrolPoint = patrolPoints[currentPatrolIndex];
            }

            var newPosition = Vector2.MoveTowards(transform.position, targetPatrolPoint.position, enemyData.MovementSpeed * Time.deltaTime);
            var direction = newPosition - (Vector2) transform.position;
            rb.MovePosition(newPosition);
            FaceDirection(direction);
            
            if (Vector2.Distance(transform.position, targetPatrolPoint.position) < 1f)
            {
                targetPatrolPoint = null;
            }
        }

        protected override void Attack()
        {
            base.Attack();
            var projectile = ObjectPoolManager.Instance.GetPooledObject("EnemyProjectile");
            projectile.transform.position = firePoint.position;
            projectile.GetComponent<Projectile>().SetDirection(-transform.localScale.x, projectileData); 
        }
    }
    
}