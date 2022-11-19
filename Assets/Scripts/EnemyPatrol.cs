using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("PatrolPoits")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemies")]
    [SerializeField] private Transform enemy;

    [Header("Movements Parameters")]
    [SerializeField] private float speed;

    [Header("Enemy Animator")]
    [SerializeField]private Animator anim;


    private Vector3 initScale;
    private bool movingLeft;
    private void Awake()
    {
        initScale = enemy.localScale;
    }
    private void Update()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x) 
                MoveInDirection(-1);
            else
            {
                DirectionChange();
            }
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
            {
                DirectionChange();
            }
        }
    }
    private void DirectionChange()
    {
        movingLeft = !movingLeft;
    }
    private void MoveInDirection(int _direction)
    {
        //basic patrolling script
        //make enemy face correct direction 
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x)* -_direction, initScale.y, initScale.z);
        //move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime*_direction*speed, enemy.position.y,enemy.position.z);
    }
}
