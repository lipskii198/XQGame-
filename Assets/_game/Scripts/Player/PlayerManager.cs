﻿using System.Collections;
using _game.Scripts.Enemies.Core;
using _game.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _game.Scripts.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private int coins;
        [SerializeField] private float currentHealth;
        [SerializeField] private bool isPlayerInvulnerable;
        [SerializeField] private bool isPlayerDead;
        [SerializeField] private bool isCastOnCooldown;
        [SerializeField] private bool isBeingHit;
        [SerializeField] private CharacterStats characterStats;
        [Header("Health Settings")]
        [SerializeField] private GameObject healthBar;
        [SerializeField] private Image healthBarFill;
        [SerializeField] private TMP_Text healthBarText;
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
        private SpellsManager spellsManager;
        private CharacterController2D controller;

        public bool IsBeingHit => isBeingHit;
        private void Start()
        {
            controller = GetComponent<CharacterController2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spellsManager = GetComponent<SpellsManager>();
            UpdateStats(recalculateStats:true);
            
            onPlayerDeath.AddListener(GameManager.Instance.OnPlayerDeath);
        }
        
        private void Update()
        {
            // healthBarFill.fillAmount = currentHealth / GetCharacterStats.health;
            // healthBarText.text = $"{currentHealth} / {GetCharacterStats.health}";
            
            if (Input.GetButtonDown("Fire1") && !isCastOnCooldown)
            {
                StartCoroutine(CastSpell());
            }
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
            if (knockBack && controller.IsGrounded && currentTarget != null) 
            {
                const int knockBackForce = 50;
                rb.AddForce(-knockBackDirection.normalized * knockBackForce, ForceMode2D.Impulse);
                Debug.Log("Knocked");
            }

            isBeingHit = false;
        }
        
        
        public void Heal(float healAmount)
        {
            currentHealth += healAmount;
        }
        
        public void Die()
        {
            healthBar.SetActive(false);
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
        
        public IEnumerator CastSpell()
        {
            isCastOnCooldown = true;
            animator.SetTrigger("Attack");
            spellsManager.Cast("Fireball");
            yield return new WaitForSeconds(spellsManager.GetCurrentProjectile.Cooldown);
            isCastOnCooldown = false;
        }
    }
}

