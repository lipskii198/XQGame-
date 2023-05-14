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

    [Header("Boulder spawn logic")]
    [SerializeField] BoxCollider2D spawnArea;
    [SerializeField] GameObject boulderPrefab;
    [SerializeField] float interval;
    [SerializeField] int boulderSpawned;
    int i;

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


        if (time > attackCooldown && isGrounded&&!meleeAttack)
        {
            time = 0;
            JumpAttack();
            meleeAttack = true;
        }
        if (landing() && canAttack)
        {
            canAttack = false;
            player.GetComponent<PlayerManager>().TakeDamage(damage, true);
            playerRB.AddForce((player.position - transform.position).normalized * 1000f, ForceMode2D.Impulse);

            Debug.Log($"Player - {player.position} Boss - {transform.position} = {(player.position - transform.position).normalized}");
            Debug.Log($"Player - {player.position} Boss - {transform.position} = {(player.position - transform.position).normalized * 1000f}");

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
        player.GetComponent<PlayerManager>().SetTarget(gameObject);

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
    private void SpawnBoulder()
    {
        for (i = 0; i < boulderSpawned; i++)
        {
            var randomX = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            var boulder = Object.Instantiate(boulderPrefab, new Vector3(randomX, spawnArea.bounds.min.y), Quaternion.identity);

        }
    }
}
