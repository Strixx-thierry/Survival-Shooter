using SurvivalShooter.Core;
using SurvivalShooter.Pooling;
using SurvivalShooter.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SurvivalShooter.Player
{
    /// <summary>Fires pooled bullets forward.</summary>
    public class PlayerShooter : MonoBehaviour
    {
        [SerializeField] ProjectilePool bulletPool;
        [SerializeField] HoldButton fireButton;
        [SerializeField] float fireInterval = 0.25f;
        [SerializeField] int damage = 1;

        float nextFireTime;

        void Update()
        {
            var game = GameManager.Instance;
            if (game == null || game.CurrentState != GameStateId.Play) return;

            bool wantsFire = fireButton.IsHeld || (Keyboard.current != null && Keyboard.current.spaceKey.isPressed);
            if (!wantsFire || Time.time < nextFireTime) return;

            nextFireTime = Time.time + fireInterval;
            Vector3 origin = transform.position + transform.forward * 0.2f - transform.up * 0.1f;
            bulletPool.Fire(origin, transform.forward, damage, Team.Player);
            GameEvents.RaiseSound(SoundCue.PlayerShoot);
        }
    }
}
