using SurvivalShooter.Core;
using UnityEngine;

namespace SurvivalShooter.Enemies
{
    public enum EnemyType { Melee, Shooter }

    /// <summary>Shared enemy health and movement.</summary>
    public abstract class EnemyBase : MonoBehaviour, IDamageable
    {
        [SerializeField] protected int maxHealth = 2;
        [SerializeField] protected float moveSpeed = 0.4f;
        [SerializeField] protected int scoreValue = 10;

        protected Transform target;
        protected DifficultySettings settings;

        int health;
        float punch;
        Vector3 baseScale;

        public Team Team => Team.Enemy;
        public bool IsAlive => health > 0;

        protected float Speed => moveSpeed * settings.enemySpeedMultiplier;

        public void Init(Transform player, DifficultySettings difficulty)
        {
            target = player;
            settings = difficulty;
            health = maxHealth;
            baseScale = transform.localScale;
        }

        void OnEnable() => GameEvents.StateChanged += OnStateChanged;
        void OnDisable() => GameEvents.StateChanged -= OnStateChanged;

        // Wipe enemies when play ends.
        void OnStateChanged(GameStateId state)
        {
            if (state != GameStateId.Play) Destroy(gameObject);
        }

        public void TakeDamage(int amount)
        {
            if (!IsAlive) return;
            health -= amount;
            punch = 0.25f;
            if (health <= 0) Die();
        }

        protected virtual void Die()
        {
            GameEvents.RaiseEnemyKilled(scoreValue);
            Destroy(gameObject);
        }

        void Update()
        {
            if (punch > 0f)
            {
                punch -= Time.deltaTime;
                transform.localScale = baseScale * (1f + Mathf.Max(0f, punch));
            }

            if (!IsAlive || target == null) return;
            Act();
        }

        protected abstract void Act();

        protected float PlanarDistance()
        {
            Vector3 offset = target.position - transform.position;
            offset.y = 0f;
            return offset.magnitude;
        }

        protected void FaceTarget()
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0f;
            if (direction.sqrMagnitude > 0.0001f) transform.rotation = Quaternion.LookRotation(direction);
        }

        protected void MoveTowardTarget()
        {
            transform.position += transform.forward * (Speed * Time.deltaTime);
        }

        protected int ScaledDamage(int baseDamage) => Mathf.RoundToInt(baseDamage * settings.enemyDamageMultiplier);
    }
}
