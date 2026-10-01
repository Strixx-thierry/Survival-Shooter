using SurvivalShooter.Core;
using UnityEngine;

namespace SurvivalShooter.Pooling
{
    /// <summary>Scene wrapper for projectile pool.</summary>
    public class ProjectilePool : MonoBehaviour
    {
        [SerializeField] Projectile prefab;
        [SerializeField, Min(1)] int poolSize = 30;

        ObjectPool<Projectile> pool;

        void Awake()
        {
            pool = new ObjectPool<Projectile>(prefab, transform, poolSize, onRelease: p => p.ResetState());
        }

        void OnEnable() => GameEvents.StateChanged += OnStateChanged;
        void OnDisable() => GameEvents.StateChanged -= OnStateChanged;

        /// <summary>Fires one pooled bullet.</summary>
        public bool Fire(Vector3 position, Vector3 direction, int damage, Team team)
        {
            Projectile p = pool.Get();
            if (p == null) return false;
            p.Fire(position, direction, damage, team, pool.Release);
            return true;
        }

        void OnStateChanged(GameStateId state)
        {
            if (state != GameStateId.Play) pool.ReleaseAll();
        }
    }
}
