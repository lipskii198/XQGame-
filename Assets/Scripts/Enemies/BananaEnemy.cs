using System.Collections.Generic;
using Enemies.Core;
using ObjectPooling;
using ScriptableObjects;
using UnityEngine;

namespace Enemies
{
    public class BananaEnemy : EnemyBase
    {
        [SerializeField] private List<Transform> patrolPoints;
        [SerializeField] private Transform targetPatrolPoint;
        [SerializeField] private int currentPatrolIndex;
        [SerializeField] private Transform firePoint;

        private void Start()
        {
            patrolPoints = new List<Transform>
            {
                new GameObject("PointLeft").transform,
                new GameObject("PointRight").transform
            };

            patrolPoints[0].position = new Vector2(transform.position.x - 5, 2);
            patrolPoints[1].position = new Vector2(transform.position.x + 5, 2);


            foreach (var patrolPoint in patrolPoints)
            {
                patrolPoint.parent = parentHolder;
            }
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
            projectile.GetComponent<Projectile>().SetDirection(-transform.localScale.x, Resources.Load<SpellData>("ScriptableObjects/Spells/Fireball"));
        }
    }
}