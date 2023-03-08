using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyBossData", menuName = "ScriptableObjects/EnemyBossData", order = 0)]
    public class EnemyBossData : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private string bossName;
        [SerializeField] private int phasesAmount;
        [SerializeField] private int[] phasesHealthThreshold;
        [SerializeField] private int[] phasesMovementSpeed;
        [SerializeField] private float[] phasesAttackCooldown;
        [SerializeField] private int[] phasesAttackRange;
        [SerializeField] private int[] phasesDamage;

        public int Id => id;
        public string BossName => bossName;
        public int PhasesAmount => phasesAmount;
        public int[] PhasesHealthThreshold => phasesHealthThreshold;
        public int[] PhasesMovementSpeed => phasesMovementSpeed;
        public float[] PhasesAttackCooldown => phasesAttackCooldown;
        public int[] PhasesAttackRange => phasesAttackRange;
        public int[] PhasesDamage => phasesDamage;
    }
}