using UnityEngine;

namespace SurvivalShooter.Core
{
    public enum Difficulty { Easy, Normal, Hard }

    /// <summary>Difficulty values per level.</summary>
    [CreateAssetMenu(menuName = "Survival Shooter/Difficulty", fileName = "Difficulty_Normal")]
    public class DifficultySettings : ScriptableObject
    {
        public Difficulty level = Difficulty.Normal;
        [Min(10f)] public float sessionSeconds = 90f;
        [Min(1)] public int playerMaxHealth = 100;
        [Min(0.2f)] public float spawnInterval = 3f;
        [Min(1)] public int maxEnemiesAlive = 6;
        public float enemySpeedMultiplier = 1f;
        public float enemyDamageMultiplier = 1f;
    }
}
