using System;
using Managers;
using UnityEngine;


[Obsolete("Replaced by Dev/CharacterController2D Soon™")]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float wallJumpCooldown;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private float horizontalInput;
    private float cooldownTimer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private SpellsManager spellsManager;
    private PlayerManager playerManager;
    private int possibleJump;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        spellsManager = GetComponent<SpellsManager>();
        playerManager = GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    
        //flipping animation
        transform.localScale = horizontalInput switch
        {
            > 0.01f => new Vector2(3, transform.localScale.y),
            < -0.01f => new Vector2(-3, transform.localScale.y),
            _ => transform.localScale
        };

        if (Input.GetMouseButton(0) && cooldownTimer > playerManager.GetCharacterStats.attackSpeed && CanAttack())
            BeginAttack();
        
        cooldownTimer += Time.deltaTime;

        //setting on running animation
        anim.SetBool("running", horizontalInput != 0);


        //walljump logic
        if (wallJumpCooldown >0.2f)
        {

            body.velocity = new Vector2(horizontalInput * playerManager.GetCharacterStats.movementSpeed, body.velocity.y);
            if(OnWall() && !IsGrounded())
            {
                body.velocity = Vector2.zero; 
            }
            if (Input.GetKeyDown(KeyCode.Space) && (IsGrounded() || possibleJump>1)) 
            {
                Jump();
            }
            else
            {
                body.gravityScale = 1;
            }
        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }
        if(IsGrounded())
        {
            possibleJump =2; 
        }


    }

    //jump
    private void Jump()
    {
        possibleJump -= 1;           
        body.velocity = new Vector2(body.velocity.x, playerManager.GetCharacterStats.jumpSpeed);
        if (IsGrounded() )
        {
 
        }
        /*else if (OnWall() && !IsGrounded())
        {

            body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 1, 3);
            wallJumpCooldown = 0;

        }
        */
    }
    //fixing infinite jump and addign wall jump
    private bool IsGrounded()
    {
        var raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.01f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool OnWall()
    {
        var raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.01f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool CanAttack()
    {
        return horizontalInput == 0 && IsGrounded() && !OnWall();
    }

    private void BeginAttack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;
        spellsManager.Cast("Fireball");
    }
    
    //TODO: Update to work with new WIP stats system
    /*
    public void RageUP(float rageDuration, float shootingSpeedreduce)
    {
        float prevSpeed = attackCooldown;
        attackCooldown = prevSpeed * (100 - shootingSpeedreduce) / 100;
        StartCoroutine(RageTime(rageDuration, prevSpeed));
    }
    private IEnumerator RageTime(float rageDuration, float prevSpeed)
    {
        yield return new WaitForSeconds(rageDuration); // waits before giving back old shooting speed 
        attackCooldown = prevSpeed;
    }
    */

}