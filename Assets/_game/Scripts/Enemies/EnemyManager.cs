using _game.Scripts.ObjectPooling;
using _game.Scripts.ScriptableObjects;
using UnityEngine;

namespace _game.Scripts.Enemies
{
    public class EnemyManager : MonoBehaviour
    {

        [SerializeField] private EnemyData enemyData;
        [SerializeField] private ProjectileData currentProjectile;
        [SerializeField] private float currentHealth;
        [Header("range attack")]
        [SerializeField] private Transform firepoint;

        [Header("collider parameters")]
        [SerializeField] private float colliderDistance;
        [SerializeField] private BoxCollider2D boxcollider;

        [Header("player layer")]
        [SerializeField] private LayerMask playerLayer;
        private float cooldownTimer = Mathf.Infinity;

        //references 
        private Animator anim;
        private EnemyController enemyController;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyController = GetComponent<EnemyController>();
        }

        private void Start()
        {
            currentHealth = enemyData.Health;
        }

        private void Update()
        {
            cooldownTimer += Time.deltaTime;
            //attack only when player in sight 
            if (PlayerInSight())
            {
                enemyController.Idle();
                if (cooldownTimer >= enemyData.AttackSpeed)
                {
                    RangedAttack();
                }
            }
            if (enemyController != null)
            {
                enemyController.enabled = !PlayerInSight();
            }
            
            if (currentHealth <= 0)
            {
                anim.SetTrigger("Die");
                Destroy(gameObject, 1f);
                enemyController.enabled = false;
                this.enabled = false;
            }
        }
        private void RangedAttack()
        {
            cooldownTimer = 0;
            // fireballs[FindFireBall()].transform.position = firepoint.position;
            // fireballs[FindFireBall()].GetComponent<EnemyProjectile>().ActivateProjectile();
            anim.SetTrigger("Attack");
            Debug.Log($"Enemy {gameObject.name} is attacking with {currentProjectile.name} projectile");
            var projectile = ObjectPoolManager.Instance.GetPooledObject("EnemyProjectile");
            projectile.transform.position = firepoint.position;
            projectile.GetComponent<Projectile>().SetDirection(-transform.localScale.x, currentProjectile);
        }
        private bool PlayerInSight()
        {
            //will detect if there are enemeis in the hitzone 
            var hit = Physics2D.BoxCast(boxcollider.bounds.center + transform.right * (enemyData.AttackRange * -transform.localScale.x * colliderDistance),
                new Vector3(boxcollider.bounds.size.x * enemyData.AttackRange, boxcollider.bounds.size.y, boxcollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
            return hit.collider != null;
        }
        private void OnDrawGizmos()
        {
            //will color the hitzone of enemies red 
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(boxcollider.bounds.center + transform.right * enemyData.AttackRange * -transform.localScale.x * colliderDistance, new Vector3(boxcollider.bounds.size.x * enemyData.AttackRange, boxcollider.bounds.size.y, boxcollider.bounds.size.z));
        }
        
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
        }
    }
}

