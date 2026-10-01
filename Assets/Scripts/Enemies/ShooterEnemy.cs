using SurvivalShooter.Core;
using SurvivalShooter.Pooling;
using UnityEngine;

namespace SurvivalShooter.Enemies
{
    /// <summary>Ranged enemy, keeps distance.</summary>
    public class ShooterEnemy : EnemyBase
    {
        [SerializeField] float shootDistance = 1.0f;
        [SerializeField] int damage = 8;
        [SerializeField] float fireCooldown = 2f;
        [SerializeField] float muzzleHeight = 0.25f;

        ProjectilePool pool;
        float nextFireTime;

        public void SetPool(ProjectilePool projectilePool) => pool = projectilePool;

        protected override void Act()
        {
            FaceTarget();

            if (PlanarDistance() > shootDistance)
            {
                MoveTowardTarget();
                return;
            }

            if (Time.time < nextFireTime || pool == null) return;
            nextFireTime = Time.time + fireCooldown;

            Vector3 muzzle = transform.position + Vector3.up * muzzleHeight;
            pool.Fire(muzzle, target.position - muzzle, ScaledDamage(damage), Team.Enemy);
            GameEvents.RaiseSound(SoundCue.EnemyShoot);
        }
    }
}
