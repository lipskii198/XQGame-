using System.Collections;
using _game.Scripts.Enemies.Core;
using _game.Scripts.Managers;
using _game.Scripts.Player;
using _game.Scripts.ScriptableObjects;
using UnityEngine;

namespace _game.Scripts
{
    public class Projectile : MonoBehaviour
    {
        public GameObject owner;
        
        private float direction;
        private bool hit;
        private ProjectileData projectileData;
        private BoxCollider2D boxCollider;
        private Animator anim;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            anim = GetComponent<Animator>();
        }
        
        

        private void Update()
        {
            if (hit) return;
            var movementSpeed = projectileData.Speed * Time.deltaTime * direction;
            transform.Translate(movementSpeed, 0, 0);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (hit) return;

            switch (collision.tag)
            {
                case "Enemy":
                    
                    if (collision.GetComponent<EnemyBase<EnemyData>>().IsDead)
                    {
                        IgnoreCollisionWithTarget(collision);
                        return;
                    }
                    
                    var enemy = collision.GetComponent<EnemyBase<EnemyData>>();
                    enemy.TakeDamage(projectileData.Damage);
                    GameManager.Instance.GetLevelManager.GetPlayer.GetComponent<PlayerManager>()
                        .SetTarget(collision.gameObject);
                    break;
                
                case "EnemyBoss":
                    
                    if (collision.GetComponent<EnemyBase<EnemyBossData>>().IsDead)
                    {
                        IgnoreCollisionWithTarget(collision);
                        return;
                    }
                    
                    var enemyBoss = collision.GetComponent<EnemyBase<EnemyBossData>>();
                    enemyBoss.TakeDamage(projectileData.Damage);
                    GameManager.Instance.GetLevelManager.GetPlayer.GetComponent<PlayerManager>()
                        .SetTarget(collision.gameObject);
                    break;
                
                case "Player":
                    collision.GetComponent<PlayerManager>().TakeDamage(projectileData.Damage);
                    break;
            }
            hit = true;
            anim.SetTrigger("explosion");
            boxCollider.enabled = false;
        }

        private void IgnoreCollisionWithTarget(Collider2D target)
        {
            Physics2D.IgnoreCollision(boxCollider, target);
        }

        public void Cast(float direction, ProjectileData projectileData, GameObject owner)
        {
            this.projectileData = projectileData;
            this.direction = direction;
            this.owner = owner;
            gameObject.SetActive(true);
            hit = false;
            transform.Translate(Vector3.forward * (this.projectileData.Speed * Time.deltaTime));
            boxCollider.enabled = true;
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            IgnoreCollisionWithTarget(owner.GetComponent<Collider2D>());
            StartCoroutine(DestroyAfterSeconds(projectileData.TimeToLive));
        }

        private IEnumerator DestroyAfterSeconds(float value)
        {
            yield return new WaitForSeconds(value);
            Deactivate();
        }
    }
}
