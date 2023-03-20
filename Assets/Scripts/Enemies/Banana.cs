using System.Collections.Generic;
using Enemies.Core;
using UnityEngine;

namespace Enemies
{
    public class Banana : EnemyBase
    {
        [SerializeField] private List<Transform> patrolPoints;
        [SerializeField] private Transform targetPatrolPoint;
        [SerializeField] private int currentPatrolIndex;
        protected override void Patrol()
        {
            if (targetPatrolPoint == null)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
                targetPatrolPoint = patrolPoints[currentPatrolIndex];
            }

            var newPosition = Vector2.MoveTowards(transform.position, targetPatrolPoint.position, enemyData.Speed * Time.deltaTime * 3);
            rb.MovePosition(newPosition);

            if (Vector2.Distance(transform.position, targetPatrolPoint.position) < 0.1f)
            {
                targetPatrolPoint = null;
            }
        }
    }
}