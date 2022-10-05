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
        //get animator to be able to change animation and playermovement to see when its still and can attack 
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
    //uses firePoint as base for starting the fireball and gives fireball same direction as player
    private void Attack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        fireball[FindFireball()].transform.position = firePoint.position;
        fireball[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));

    }
    //find the first non active fireball (that hasn't been shot) 
    private int FindFireball()
    {
        for(int i=0; i < fireball.Length; i++)
        {
            if (!fireball[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
