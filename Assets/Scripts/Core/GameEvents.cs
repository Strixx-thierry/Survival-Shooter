using System;

namespace SurvivalShooter.Core
{
    public enum SoundCue { PlayerShoot, PlayerDeath, EnemySpawn, EnemyShoot, MeleeHit, UiClick }

    /// <summary>Observer pattern event hub.</summary>
    public static class GameEvents
    {
        public static event Action<GameStateId> StateChanged;
        public static event Action<int, int> PlayerHealthChanged;
        public static event Action<int> ScoreChanged;
        public static event Action<float> TimeRemainingChanged;
        public static event Action<int> EnemyKilled;
        public static event Action GameWorldPlaced;
        public static event Action PlayerDied;
        public static event Action<SoundCue> SoundRequested;

        public static void RaiseStateChanged(GameStateId id) => StateChanged?.Invoke(id);
        public static void RaisePlayerHealthChanged(int current, int max) => PlayerHealthChanged?.Invoke(current, max);
        public static void RaiseScoreChanged(int score) => ScoreChanged?.Invoke(score);
        public static void RaiseTimeRemainingChanged(float seconds) => TimeRemainingChanged?.Invoke(seconds);
        public static void RaiseEnemyKilled(int scoreValue) => EnemyKilled?.Invoke(scoreValue);
        public static void RaiseGameWorldPlaced() => GameWorldPlaced?.Invoke();
        public static void RaisePlayerDied() => PlayerDied?.Invoke();
        public static void RaiseSound(SoundCue cue) => SoundRequested?.Invoke(cue);
    }
}
