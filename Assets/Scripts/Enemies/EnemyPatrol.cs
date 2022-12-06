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
    private Vector3 initScale;
    private bool movingLeft;


    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer; 

    [Header("Enemy Animator")]
    [SerializeField]private Animator anim;




    private void Awake()
    {
        initScale = enemy.localScale;
        anim = GetComponent<Animator>();

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
        anim.SetBool("Move", false);
        idleTimer += Time.deltaTime;
        if(idleTimer>idleDuration)
            movingLeft = !movingLeft;
    }
    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        //basic patrolling script
        //make enemy face correct direction 
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x)* -_direction, initScale.y, initScale.z);
        //move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime*_direction*speed, enemy.position.y,enemy.position.z);
        //set moving animation to true 
        anim.SetBool("Move", true); 
    }
}
