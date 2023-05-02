using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Esfand : MonoBehaviour
{
    [SerializeField] float jumpHeight;
    [SerializeField] Transform player;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform boss;
    [SerializeField] Vector2 boxSize;
    [SerializeField] float attackCooldown;
    private Rigidbody2D bossRB;
    private bool isGrounded;
    private float time;
    private float startY;

    private void Awake()
    {
        bossRB = GetComponent<Rigidbody2D>();
        time = 0;
        startY = boss.position.y;

    }
    private void Update()
    {
        time += Time.deltaTime;
        isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, groundLayer);


        print("landing is" + landing());

        

        if (time>attackCooldown)
        {
            time = 0;
            JumpAttack();

        }
    }
    private void JumpAttack()
    {
        float distanceFromPlayer = player.position.x - transform.position.x;


        bossRB.AddForce(new Vector2(distanceFromPlayer, jumpHeight), ForceMode2D.Impulse);

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(groundCheck.position, boxSize);
    }
    private bool landing()
    {          
        if (boss.position.y < startY + 0.4 && bossRB.velocity.y < 0)
            return true;
        return false;
    }
}
