using SurvivalShooter.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SurvivalShooter.Core
{
    /// <summary>Editor test keys: P, H, E, K.</summary>
    public class DebugFlow : MonoBehaviour
    {
        void Update()
        {
            var keys = Keyboard.current;
            if (keys == null) return;

            if (keys.pKey.wasPressedThisFrame) GameEvents.RaiseGameWorldPlaced();
            if (keys.eKey.wasPressedThisFrame) GameEvents.RaiseEnemyKilled(10);
            if (keys.kKey.wasPressedThisFrame) GameEvents.RaisePlayerDied();
            if (keys.hKey.wasPressedThisFrame) FindFirstObjectByType<PlayerHealth>()?.TakeDamage(10);
        }
    }
}
