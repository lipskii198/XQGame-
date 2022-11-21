using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private int coins;
        [SerializeField] private CharacterStats characterStats;
        [Header("Events")] 
        public UnityEvent onStatsUpdated;

        private void Start()
        {
            characterStats.CalculateOverallStats();
        }

        public void UpdateCoins(int coinAmount)
        {
            coins = coinAmount;
        }

        public CharacterStats GetCharacterStats => characterStats;
    }
}

