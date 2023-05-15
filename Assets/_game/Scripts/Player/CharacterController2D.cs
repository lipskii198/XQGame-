using UnityEngine;

namespace _game.Scripts.Player
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
        private bool charging = false;
        private float startCharge = 0.0f;
        private Rigidbody2D rb;
        private Animator animator;
        private PlayerManager playerManager;

        private float _fallSpeadYDampingChangeThreshold;

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
            _fallSpeadYDampingChangeThreshold = CameraManager.instance._fallSpeadYDampingChangeThreshold;
            playerManager = GetComponent<PlayerManager>();
        }

        private void Update()
        {   
            //Charged jump
            if (Input.GetKey(KeyCode.LeftShift)) {
                Debug.Log("charging");   
                if (!charging) {
                    startCharge = Time.time;
                    charging = true;
                }
                //Get time key was pressed
            } else if (charging) {
                charging=false;
                //Get timestamp,
                float chargedForce = (Time.time - startCharge) * jumpForce;
                chargedForce = Mathf.Clamp(chargedForce, 0.0f, 40.0f);
                Debug.Log("JUMPED with force " + chargedForce);
                rb.velocity = new Vector2(rb.velocity.x, chargedForce); 
            }
            else if (Input.GetButtonDown("Jump"))
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
            if (rb.velocity.y < _fallSpeadYDampingChangeThreshold && !CameraManager.instance.isLerpingYDamping && !CameraManager.instance.lerpedFromPlayerFalling)
            {
                CameraManager.instance.LerpYDamping(true);
            }

            if (rb.velocity.y >= 0f && !CameraManager.instance.isLerpingYDamping && CameraManager.instance.lerpedFromPlayerFalling)
            {
                CameraManager.instance.lerpedFromPlayerFalling = false;
                CameraManager.instance.LerpYDamping(false);
            }
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