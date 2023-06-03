using _game.Scripts.Player;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace _game.Scripts.Managers
{
    public class HudManager : MonoBehaviour
    {
        [SerializeField] private bool isInitialized; // Initialized for current level only!!
        
        [SerializeField] private GameObject playerDeathScreen;

        [Header("Player Hud")]
        [SerializeField] private GameObject playerHud;
        [SerializeField] private GameObject playerHealthBar;
        [SerializeField] private Image playerHealthBarFill;
        [SerializeField] private TMP_Text playerHealthBarText;


        private PlayerManager playerManager;
        private LevelManager levelManager;
        private void Start()
        {
            levelManager = GameManager.Instance.GetLevelManager;
            
            Debug.Log($"[{GetType().Name}] Initialized");
        }
        
        private void Update()
        {
            if (!isInitialized) return;
            
            HandlePlayerHealthBar();
        }

        public void Initialize()
        {
            playerManager = levelManager.GetPlayer.GetComponent<PlayerManager>();
            playerHud.SetActive(true);
            playerHealthBar.SetActive(true);
            
            isInitialized = true;
        }

        public void Reset()
        {
            isInitialized = false;
            playerHud.SetActive(false);
            playerHealthBar.SetActive(false);
            playerDeathScreen.SetActive(false);
        }
        

        private void HandlePlayerHealthBar()
        {
            playerHealthBarFill.fillAmount = playerManager.CurrentHealth /
                                             playerManager.GetCharacterStats.health;
            playerHealthBarText.text = $"{playerManager.CurrentHealth} / {playerManager.GetCharacterStats.health}";
        }

        public void ShowPlayerDeathScreen()
        {
            playerDeathScreen.SetActive(true);
            Debug.Log("Player died");
        }
    }
}