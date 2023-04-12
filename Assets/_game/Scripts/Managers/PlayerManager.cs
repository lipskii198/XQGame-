using System.Collections;
using _game.Scripts.Enemies.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _game.Scripts.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private int coins;
        [SerializeField] private float currentHealth;
        [SerializeField] private bool isPlayerInvulnerable;
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
        private EnemyBase currentTarget;
        private SpellsManager spellsManager;
        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spellsManager = GetComponent<SpellsManager>();
            UpdateStats(recalculateStats:true);
            
            onPlayerDeath.AddListener(GameManager.Instance.OnPlayerDeath);
        }

        private void Update()
        {
            healthBarFill.fillAmount = currentHealth / GetCharacterStats.health;
            healthBarText.text = $"{currentHealth} / {GetCharacterStats.health}";
            
            if (Input.GetButtonDown("Fire1"))
            {
                CastSpell();
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

        public void TakeDamage(float damage)
        {
            if (isPlayerInvulnerable) return;
            
            if (currentHealth - damage <= 0)
            {
                currentHealth = 0;
                Die();
                return;
            }
            
            currentHealth -= damage;
            StartCoroutine(BeginInvulnerability());
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
        
        public void SetTarget(EnemyBase enemyBase)
        {
            if (currentTarget != null)
            {
                currentTarget.ToggleHealthBar(false);
            }
            currentTarget = enemyBase;
            enemyBase.ToggleHealthBar(true);
        }
        
        public void CastSpell()
        {
            animator.SetTrigger("Attack");
            spellsManager.Cast("Fireball");
        }
    }
}

