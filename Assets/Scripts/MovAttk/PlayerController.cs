using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float wallJumpCooldown;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private float horizontalInput;
    private float cooldownTimer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private SpellsManager spellsManager;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        spellsManager = GetComponent<SpellsManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //flipping animation
        transform.localScale = horizontalInput switch
        {
            > 0.01f => Vector3.one,
            < -0.01f => new Vector3(-1, 1, 1),
            _ => transform.localScale
        };

        if (Input.GetMouseButton(0)&& cooldownTimer > attackCooldown && CanAttack())
            BeginAttack();
        
        cooldownTimer += Time.deltaTime;

        //setting on running animation
        anim.SetBool("running", horizontalInput != 0);

        //walljump logic
        if (wallJumpCooldown >0.2f)
        {

            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            if(OnWall() && !IsGrounded())
            {
                body.velocity = Vector2.zero; 
            }
            if (Input.GetKey(KeyCode.Space))
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
            wallJumpCooldown = wallJumpCooldown + Time.deltaTime;
        }


    }
    //jump
    private void Jump()
    {
        if (IsGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }
        else if (OnWall() && !IsGrounded())
        {

            body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 1, 3);
            wallJumpCooldown = 0;

        }
    }
    //fixing infinite jump and addign wall jump
    private bool IsGrounded()
    {
        var raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.03f, groundLayer);
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
        spellsManager.Cast();
    }
}