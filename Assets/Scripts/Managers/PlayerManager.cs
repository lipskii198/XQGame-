using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Managers
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
        
        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            UpdateStats(recalculateStats:true);
        }

        private void Update()
        {
            //healthBarFill.fillAmount = currentHealth / GetCharacterStats.health;
            //healthBarText.text = $"{currentHealth} / {GetCharacterStats.health}";
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                TakeDamage(1f);
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
            
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, characterStats.health);
            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(BeginInvulnerability());
            }
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
        
    }
}

