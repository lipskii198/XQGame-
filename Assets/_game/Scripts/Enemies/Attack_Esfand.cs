using System.Collections;
using System.Collections.Generic;
using _game.Scripts.Managers;
using _game.Scripts.Player;
using UnityEngine;
public class Attack_Esfand : MonoBehaviour
{
    [SerializeField] float jumpHeight;
    [SerializeField] float damage;
    [SerializeField] Transform player;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform enemyCheck;
    [SerializeField] Transform boss;
    [SerializeField] Vector2 boxSize;
    [SerializeField] Vector2 hitSize;
    [SerializeField] Vector2 meleeSize;
    [SerializeField] float attackCooldown;
    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] Vector2 knockBack;
    private Rigidbody2D bossRB;
    private bool isGrounded;
    private bool canAttack;
    private float time;
    private float startY;
    private bool meleeAttack;
    private int i;


    [Header("boulder spawn logic")]
    [SerializeField] BoxCollider2D boulderSpawnArea;
    [SerializeField] int boulderSpawned;
    [SerializeField] GameObject boulderPrefab;

    private void Awake()
    {
        bossRB = GetComponent<Rigidbody2D>();
        time = 0;
        startY = boss.position.y;
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
        meleeAttack = false;



    }
    private void Update()
    {
        time += Time.deltaTime;
        isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, groundLayer);
        canAttack = Physics2D.OverlapBox(groundCheck.position, hitSize, 0, playerLayer);
        SpawnBoulders();



        if (time > attackCooldown && isGrounded&&!meleeAttack)
        {
            time = 0;
            JumpAttack();
            meleeAttack = true;
        }
        if (landing() && canAttack)
        {
            player.GetComponent<PlayerManager>().TakeDamage(damage);
            playerRB.AddForce(knockBack, ForceMode2D.Impulse);
            
        }

        if(time>attackCooldown&& meleeAttack)
        {
            time = 0;
            MeleeAttack();
            meleeAttack = false;
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
        Gizmos.color = Color.red;
        Gizmos.DrawCube(groundCheck.position, hitSize);
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(enemyCheck.position, meleeSize);
    }
    private bool landing()
    {
        if (boss.position.y < startY + 2 && bossRB.velocity.y < 0)
            return true;
        return false;
    }
    private void MeleeAttack()
    {
            Vector2 scale = transform.localScale;
            scale.x = -Mathf.Sign(player.position.x - boss.position.x) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        if(Physics2D.OverlapBox(enemyCheck.position, meleeSize, 0, playerLayer))
        {
            player.GetComponent<PlayerManager>().TakeDamage(damage);
        }
            

    }
    private void SpawnBoulders()
    {
        for (i = 0; i < boulderSpawned; i++)
        {
            var randomX = Random.Range(boulderSpawnArea.bounds.min.x, boulderSpawnArea.bounds.max.x);
            var boulder = Object.Instantiate(boulderPrefab, new Vector3(randomX, boulderSpawnArea.bounds.min.y), Quaternion.identity);
        }
    }
}
