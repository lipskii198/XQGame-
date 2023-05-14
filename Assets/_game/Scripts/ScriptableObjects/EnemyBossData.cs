using UnityEngine;

namespace _game.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyBossData", menuName = "ScriptableObjects/EnemyBossData", order = 0)]
    public class EnemyBossData : EnemyBaseData
    {
        [SerializeField] private string bossName;
        [SerializeField] private int phasesAmount;
        [SerializeField] private int[] phasesHealthThreshold;
        [SerializeField] private int[] phasesMovementSpeed;
        [SerializeField] private float[] phasesAttackCooldown;
        [SerializeField] private int[] phasesAttackRange;
        [SerializeField] private int[] phasesDamage;

        public string BossName => bossName;
        public int PhasesAmount => phasesAmount;
        public int[] PhasesHealthThreshold => phasesHealthThreshold;
        public int[] PhasesMovementSpeed => phasesMovementSpeed;
        public float[] PhasesAttackCooldown => phasesAttackCooldown;
        public int[] PhasesAttackRange => phasesAttackRange;
        public int[] PhasesDamage => phasesDamage;
    }
}