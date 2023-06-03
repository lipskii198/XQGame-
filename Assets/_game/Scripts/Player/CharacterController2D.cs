using _game.Scripts.Managers;
using _game.Scripts.Utility;
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
        
        // Block lists
        public Blocklist MovementBlocklist { get; } = new();
        public Blocklist JumpBlocklist { get; } = new();
        
        // Block lists objects
        private object movementBlocker;
        private object jumpBlocker;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            _fallSpeadYDampingChangeThreshold = CameraManager.Instance._fallSpeadYDampingChangeThreshold;
            playerManager = GetComponent<PlayerManager>();

            Debug.Log($"[{GetType().Name}] Initialized");
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
                }
                else if (canDoubleJump)
                {
                    Jump(isDoubleJump:true);
                }
            }
            
            animator.SetBool(IsRunning, movementInput != 0);
            if (rb.velocity.y < _fallSpeadYDampingChangeThreshold && !CameraManager.Instance.isLerpingYDamping && !CameraManager.Instance.lerpedFromPlayerFalling)
            {
                CameraManager.Instance.LerpYDamping(true);
            }

            if (rb.velocity.y >= 0f && !CameraManager.Instance.isLerpingYDamping && CameraManager.Instance.lerpedFromPlayerFalling)
            {
                CameraManager.Instance.lerpedFromPlayerFalling = false;
                CameraManager.Instance.LerpYDamping(false);
            }
        }

        private void FixedUpdate()
        {
            isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);

            movementInput = Input.GetAxisRaw("Horizontal");
            
            if (!MovementBlocklist.IsBlocked())
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
            if (JumpBlocklist.IsBlocked()) return;
            
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
            if (isDoubleJump)
            {
                canDoubleJump = false;
                isDoubleJumping = true;
            }
            else
            {
                isJumping = true;
                canDoubleJump = true;
            }
        }
    }
}