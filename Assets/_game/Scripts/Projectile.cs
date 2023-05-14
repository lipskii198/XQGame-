using System.Collections;
using _game.Scripts.Enemies;
using _game.Scripts.Enemies.Core;
using _game.Scripts.Managers;
using _game.Scripts.Player;
using _game.Scripts.ScriptableObjects;
using UnityEngine;

namespace _game.Scripts
{
    public class Projectile : MonoBehaviour
    {
        private float direction;
        private bool hit;
        private ProjectileData projectileData;
        private BoxCollider2D boxCollider;
        private Animator anim;
        // Start is called before the first frame update

        void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (hit) return;
            float movementSpeed = projectileData.Speed * Time.deltaTime*direction;
            transform.Translate(movementSpeed, 0, 0);
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (hit) return;

            if (collision.CompareTag("Enemy") || collision.CompareTag("EnemyBoss"))
            {
                if (collision.GetComponent<EnemyBase<EnemyData>>().IsDead)
                {
                    IgnoreCollisionWithTarget(collision);
                    return;
                }
            }
            
            hit = true;
            anim.SetTrigger("explosion");
            
            switch (collision.tag)
            {
                // Player -> PlayerManager.TakeDamage
                // Enemy -> EnemyManager.TakeDamage
            
                case "Enemy":
                    var enemy = collision.GetComponent<EnemyBase<EnemyData>>();
                    enemy.TakeDamage(projectileData.Damage);
                    LevelManager.Instance.GetPlayer.GetComponent<PlayerManager>().SetTarget(collision.gameObject);
                    break;
                case "Player":
                    collision.GetComponent<PlayerManager>().TakeDamage(projectileData.Damage);
                    break;
                case "EnemyBoss":
                    break;
            }
            boxCollider.enabled = false;
        }
        
        private void IgnoreCollisionWithTarget(Collider2D target)
        {
            Physics2D.IgnoreCollision(boxCollider, target);
        }
        
        public void SetDirection(float _direction, ProjectileData projectileData)
        {
            this.projectileData = projectileData;
            direction = _direction;
            gameObject.SetActive(true);
            hit = false;
            transform.Translate((Vector3.forward * this.projectileData.Speed) * Time.deltaTime); 
            boxCollider.enabled = true;
            
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    
        private void OnEnable()
        {
            StartCoroutine(DestroyAfterSeconds(projectileData.TimeToLive));
        }
    
    
        private IEnumerator DestroyAfterSeconds(float value)
        {
            yield return new WaitForSeconds(value); // Wait for [value] seconds without blocking the current thread
            Deactivate();
        }
    }
}
