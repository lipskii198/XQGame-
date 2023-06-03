using System;
using System.Collections;
using _game.Scripts.Managers;
using _game.Scripts.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace _game.Scripts.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private int coins;
        [SerializeField] private float currentHealth;
        [SerializeField] private bool isPlayerInvulnerable;
        [SerializeField] private bool isPlayerDead;
        [SerializeField] private bool isBeingHit;
        [SerializeField] private bool isKnockBackEnabled = false; // Too many issues with knockback, disabling for now
        [SerializeField] private CharacterStats characterStats;
        [Header("Invulnerability Settings")]
        [SerializeField] private float invulnerabilityTime;
        [SerializeField] private float invulnerabilityFlashTime;
        [Header("Events")] 
        public UnityEvent onStatsUpdated;
        public UnityEvent onPlayerDeath;
        
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rb;
        private Animator animator;
        private GameObject currentTarget;
        private CharacterController2D controller;
        private StateMachine stateMachine;
        public bool IsBeingHit => isBeingHit;
        public float CurrentHealth => currentHealth;
        
        
        // Block lists
        public Blocklist AttackBlocklist { get; } = new();

        // Block lists objects
        private object attackBlocker;

        // Animation Hashes
        private static readonly int Attack = Animator.StringToHash("Attack");

        private void Start()
        {
            stateMachine = GetComponent<StateMachine>();
            controller = GetComponent<CharacterController2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            UpdateStats(recalculateStats:true);
            
            onPlayerDeath.AddListener(GameManager.Instance.OnPlayerDeath);
            
            Debug.Log($"[{GetType().Name}] Initialized");
        }

        private void Update()
        {
            stateMachine.Tick();
        }

        private void FixedUpdate()
        {
            stateMachine.FixedTick();
        }

        public void UpdateCoins(int coinAmount)
        {
            coins = coinAmount;
        }

        public CharacterStats GetCharacterStats => characterStats;
        
        public void UpdateStats(bool recalculateStats=false)
        {
            if (recalculateStats)
            {
                characterStats.CalculateOverallStats();
            }
            currentHealth = characterStats.health;
            onStatsUpdated.Invoke();
        }

        public void TakeDamage(float damage, bool knockBack = false,  Vector2 knockBackDirection = default)
        {
            if (isPlayerInvulnerable) return;

            isBeingHit = true;
            controller.JumpBlocklist.RegisterBlocker(attackBlocker = new object());
            
            if (currentHealth - damage <= 0)
            {
                currentHealth = 0;
                Die();
                return;
            }
            
            currentHealth -= damage;
            StartCoroutine(BeginInvulnerability());
            
            //BUG: player knocks down when hit during jump
            // controller.IsGrounded until bug is fixed
            if (isKnockBackEnabled && knockBack && controller.IsGrounded) 
            {
                const int knockBackForce = 50;
                rb.AddForce(-knockBackDirection.normalized * knockBackForce, ForceMode2D.Impulse);
                Debug.Log("Knocked");
            }

            isBeingHit = false;
            controller.JumpBlocklist.UnregisterBlocker(attackBlocker);
        }
        
        public void Heal(float healAmount)
        {
            currentHealth += healAmount;
        }
        
        public void Die()
        {
            onPlayerDeath.Invoke();
        }

        public IEnumerator BeginInvulnerability()
        {
            Debug.Log("Player is invulnerable");
            Physics2D.IgnoreLayerCollision(10, 11, true);
            isPlayerInvulnerable = true;
            for (var i = 0; i < invulnerabilityFlashTime; i++)
            {
                spriteRenderer.color = new Color(1, 0, 0,0.5f);
                yield return new WaitForSeconds(invulnerabilityTime / invulnerabilityFlashTime);
                spriteRenderer.color = new Color(1, 1, 1,1);
                yield return new WaitForSeconds(invulnerabilityTime / invulnerabilityFlashTime);
            }
            Physics2D.IgnoreLayerCollision(10, 11, false);
            isPlayerInvulnerable = false;
        }
        
        public void SetTarget(GameObject targetEnemy)
        {
            currentTarget = targetEnemy;
        }
        
        public void SetStateMachine(LevelAbilities levelAbilities)
        {
            switch (levelAbilities)
            {
                case LevelAbilities.AntsLevel:
                    stateMachine.SetState(new AntsLevelAbilities(stateMachine));
                    break;
                case LevelAbilities.EsfandLevel:
                    stateMachine.SetState(new AntsLevelAbilities(stateMachine));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(levelAbilities), levelAbilities, null);
            }
        }

        public enum LevelAbilities
        {
            AntsLevel,
            EsfandLevel,
        }

        
        private class AntsLevelAbilities : StateBase
        {
            private SpellsManager spellsManager;
            private PlayerManager playerManager;
            
            private object castFireBallBlocker;
            public AntsLevelAbilities(StateMachine stateMachine) : base(stateMachine, "AntsLevelAbilities")
            {
            }
            
            public IEnumerator CastFireBall()
            {
                playerManager.AttackBlocklist.RegisterBlocker(castFireBallBlocker);
                playerManager.animator.SetTrigger(Attack);
                
                spellsManager.Cast("Fireball");
                
                yield return new WaitForSeconds(spellsManager.GetCurrentProjectile.Cooldown);
                playerManager.AttackBlocklist.UnregisterBlocker(castFireBallBlocker);
            }

            public override void OnEnter()
            {
                spellsManager = stateMachine.GetComponent<SpellsManager>();
                playerManager = stateMachine.GetComponent<PlayerManager>();
            }

            public override void OnExit()
            {
                
            }

            public override void Tick()
            {
                if (Input.GetButtonDown("Fire1") && !playerManager.AttackBlocklist.IsBlocked())
                {
                    playerManager.StartCoroutine(CastFireBall());
                }
            }

            public override void FixedTick()
            {
                
            }
        }
    }
}

