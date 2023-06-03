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

            if (collision.CompareTag("Enemy"))
            {
                if (collision.GetComponent<EnemyBase<EnemyData>>().IsDead)
                {
                    IgnoreCollisionWithTarget(collision);
                    return;
                }
            }
            else if (collision.CompareTag("EnemyBoss"))
            {
                if (collision.GetComponent<EnemyBase<EnemyBossData>>().IsDead)
                {
                    IgnoreCollisionWithTarget(collision);
                    return;
                }
            }

            hit = true;
            anim.SetTrigger("explosion");

            switch (collision.tag)
            {
                case "Enemy":
                    var enemy = collision.GetComponent<EnemyBase<EnemyData>>();
                    enemy.TakeDamage(projectileData.Damage);
                    GameManager.Instance.GetLevelManager.GetPlayer.GetComponent<PlayerManager>()
                        .SetTarget(collision.gameObject);
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

        public void SetDirection(float direction, ProjectileData projectileData)
        {
            this.projectileData = projectileData;
            this.direction = direction;
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
            StartCoroutine(DestroyAfterSeconds(projectileData.TimeToLive));
        }

        private IEnumerator DestroyAfterSeconds(float value)
        {
            yield return new WaitForSeconds(value);
            Deactivate();
        }
    }
}
