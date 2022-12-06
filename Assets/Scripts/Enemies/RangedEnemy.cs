using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("attack parametres")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float damage;

    [Header("range attack")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;


    [Header("collider parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxcollider;

    [Header("player layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    //references 
    private Animator anim;
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponent<EnemyPatrol>();
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        //attack only when player in sight 
        if (playerInSight())
        {

            if (cooldownTimer >= attackCooldown)
            {
                RangedAttack();
            }
        }
        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !playerInSight();
        }
    }
    private void RangedAttack()
    {
        cooldownTimer = 0;
        fireballs[FindFireBall()].transform.position = firepoint.position;
        fireballs[FindFireBall()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }
    private int FindFireBall()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
            {
                return i; 
            }

        }
        return 0;
    }
    private bool playerInSight()
    {
        //will detect if there are enemeis in the hitzone 
        RaycastHit2D hit = Physics2D.BoxCast(boxcollider.bounds.center + transform.right * range * -transform.localScale.x * colliderDistance,
            new Vector3(boxcollider.bounds.size.x * range, boxcollider.bounds.size.y, boxcollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        //will color the hitzone of enemies red 
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxcollider.bounds.center + transform.right * range * -transform.localScale.x * colliderDistance, new Vector3(boxcollider.bounds.size.x * range, boxcollider.bounds.size.y, boxcollider.bounds.size.z));
    }
}

