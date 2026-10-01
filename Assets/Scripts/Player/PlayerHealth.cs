using SurvivalShooter.Core;
using UnityEngine;

namespace SurvivalShooter.Player
{
    /// <summary>Player health on the camera.</summary>
    [RequireComponent(typeof(SphereCollider))]
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        int health;
        int maxHealth = 100;

        public Team Team => Team.Player;
        public bool IsAlive => health > 0;

        void Awake()
        {
            var hitbox = GetComponent<SphereCollider>();
            hitbox.isTrigger = true;
            hitbox.radius = 0.2f;
        }

        void OnEnable() => GameEvents.StateChanged += OnStateChanged;
        void OnDisable() => GameEvents.StateChanged -= OnStateChanged;

        void OnStateChanged(GameStateId state)
        {
            if (state != GameStateId.Play) return;
            maxHealth = GameManager.Instance.Settings.playerMaxHealth;
            health = maxHealth;
            GameEvents.RaisePlayerHealthChanged(health, maxHealth);
        }

        public void TakeDamage(int amount)
        {
            if (!IsAlive) return;
            health = Mathf.Max(0, health - amount);
            GameEvents.RaisePlayerHealthChanged(health, maxHealth);
            if (health == 0)
            {
                GameEvents.RaiseSound(SoundCue.PlayerDeath);
                GameEvents.RaisePlayerDied();
            }
        }
    }
}
