using System;
using SurvivalShooter.Core;
using UnityEngine;

namespace SurvivalShooter.Pooling
{
    /// <summary>Reusable bullet for all shooters.</summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 6f;
        [SerializeField] float lifetime = 3f;

        Team ownerTeam;
        int damage;
        Vector3 direction;
        float age;
        Action<Projectile> returnToPool;

        void Awake()
        {
            var body = GetComponent<Rigidbody>();
            body.isKinematic = true;
            body.useGravity = false;
        }

        /// <summary>Re-initialises bullet after Get.</summary>
        public void Fire(Vector3 position, Vector3 dir, int dmg, Team team, Action<Projectile> onDone)
        {
            transform.SetPositionAndRotation(position, Quaternion.LookRotation(dir));
            direction = dir.normalized;
            damage = dmg;
            ownerTeam = team;
            returnToPool = onDone;
            age = 0f;
        }

        void Update()
        {
            transform.position += direction * (speed * Time.deltaTime);
            age += Time.deltaTime;
            if (age >= lifetime) returnToPool?.Invoke(this);
        }

        void OnTriggerEnter(Collider other)
        {
            var target = other.GetComponentInParent<IDamageable>();
            if (target == null)
            {
                // Solid objects stop bullet.
                if (other.GetComponentInParent<Projectile>() == null) returnToPool?.Invoke(this);
                return;
            }

            if (target.Team == ownerTeam || !target.IsAlive) return;

            target.TakeDamage(damage);
            returnToPool?.Invoke(this);
        }

        /// <summary>Clears data before reuse.</summary>
        public void ResetState()
        {
            direction = Vector3.zero;
            damage = 0;
            age = 0f;
            returnToPool = null;
        }
    }
}
