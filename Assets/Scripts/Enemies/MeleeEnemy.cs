using SurvivalShooter.Core;
using UnityEngine;

namespace SurvivalShooter.Enemies
{
    /// <summary>Close-range chaser.</summary>
    public class MeleeEnemy : EnemyBase
    {
        [SerializeField] float attackRange = 0.35f;
        [SerializeField] int damage = 10;
        [SerializeField] float attackCooldown = 1.2f;

        float nextAttackTime;

        protected override void Act()
        {
            FaceTarget();

            if (PlanarDistance() > attackRange)
            {
                MoveTowardTarget();
                return;
            }

            if (Time.time < nextAttackTime) return;
            nextAttackTime = Time.time + attackCooldown;
            GameEvents.RaiseSound(SoundCue.MeleeHit);
            target.GetComponent<IDamageable>()?.TakeDamage(ScaledDamage(damage));
        }
    }
}
