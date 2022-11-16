using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
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

        var currentFireball = ObjectPoolManager.Instance.GetPooledObject();
        currentFireball.transform.position = firePoint.position;
        currentFireball.GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }
    //gets old shooting speed, changes it and then restores it 
    public void RageUP(float rageDuration, float shootingSpeedreduce)
    {
        float prevSpeed = attackCooldown;
        attackCooldown = prevSpeed * (100 - shootingSpeedreduce)/100;
        StartCoroutine(RageTime(rageDuration, prevSpeed));
    }
    private IEnumerator RageTime(float rageDuration, float prevSpeed)
    {
        yield return new WaitForSeconds(rageDuration); // waits before giving back old shooting speed 
        attackCooldown = prevSpeed;
    }

}
