using System.Collections;
using Enemies.Core;
using Managers;
using ScriptableObjects;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private ProjectileData projectileData;
    private bool hit;
    private BoxCollider2D boxCollider;
    private Animator anim;
    private float direction;
    // Start is called before the first frame update
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if it hits we dont do anything otherwise it keeps moving 
        if (hit) return;
        float movementSpeed = projectileData.Speed * Time.deltaTime*direction;
        transform.Translate(movementSpeed, 0, 0);
    }
    //if it hits something it explodes 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hit) return;
        hit = true;
        anim.SetTrigger("explosion");
        switch (collision.tag)
        {
            // Player -> PlayerManager.TakeDamage
            // Enemy -> EnemyManager.TakeDamage
            
            case "Enemy":
                collision.GetComponent<EnemyBase>().TakeDamage(projectileData.Damage);
                break;
            case "Player":
                collision.GetComponent<PlayerManager>().TakeDamage(projectileData.Damage);
                break;
            case "EnemyBoss":
                break;
        }
        boxCollider.enabled = false;
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
