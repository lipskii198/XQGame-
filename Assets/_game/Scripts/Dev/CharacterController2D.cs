using UnityEngine;

namespace _game.Scripts.Dev
{
    public class CharacterController2D : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float doubleJumpCooldown;
        [SerializeField] private float groundCheckRadius;
        [SerializeField] private bool isGrounded;
        [SerializeField] private bool isJumping;
        [SerializeField] private bool isDoubleJumping;
        [SerializeField] private bool canDoubleJump;
        [SerializeField] private LayerMask groundLayer;

        private bool isFacingRight = true;
        private float movementInput;
        private Rigidbody2D rb;
        private Animator animator;

        #region AnimatorHashes

        private static readonly int IsRunning = Animator.StringToHash("isRunning");

        #endregion
        
        public bool IsGrounded => isGrounded;
        public bool IsJumping => isJumping;
        public bool IsDoubleJumping => isDoubleJumping;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded)
                {
                    Jump();
                    isJumping = true;
                    canDoubleJump = true;
                }
                else if (canDoubleJump)
                {
                    Jump(isDoubleJump:true);
                    isDoubleJumping = true;
                    canDoubleJump = false;
                }
            }
            
            animator.SetBool(IsRunning, movementInput != 0);
        }

        private void FixedUpdate()
        {
            isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);

            movementInput = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(movementInput * movementSpeed, rb.velocity.y);
            
            if ((movementInput > 0 && !isFacingRight) || (movementInput < 0 && isFacingRight))
            {
                Flip();
            }

        }

        private void Flip()
        {
            isFacingRight = !isFacingRight;
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        private void Jump(bool isDoubleJump = false)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}