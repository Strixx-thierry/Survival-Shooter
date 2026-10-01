namespace SurvivalShooter.Core
{
    public enum Team { Player, Enemy }

    /// <summary>Shared damage interface.</summary>
    public interface IDamageable
    {
        Team Team { get; }
        bool IsAlive { get; }
        void TakeDamage(int amount);
    }
}
