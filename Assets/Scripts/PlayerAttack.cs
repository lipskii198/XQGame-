using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireball;
    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer= Mathf.Infinity;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent < Animator>();
        playerMovement = GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)&& cooldownTimer> attackCooldown && playerMovement.CanAttack())
            Attack();
        
        cooldownTimer += Time.deltaTime;
   
    }
    private void Attack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        fireball[0].transform.position = firePoint.position;
        fireball[0].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));

    }
}
