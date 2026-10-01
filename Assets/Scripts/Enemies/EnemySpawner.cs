using System.Collections.Generic;
using SurvivalShooter.AR;
using SurvivalShooter.Core;
using UnityEngine;

namespace SurvivalShooter.Enemies
{
    /// <summary>Spawns enemies around the arena.</summary>
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] EnemyFactory factory;
        [SerializeField] ARPlacementController placement;
        [SerializeField] Transform player;
        [SerializeField] float spawnRadius = 0.7f;
        [SerializeField, Range(0f, 1f)] float meleeChance = 0.65f;

        readonly List<EnemyBase> alive = new List<EnemyBase>();
        float timer;

        void OnEnable() => GameEvents.StateChanged += OnStateChanged;
        void OnDisable() => GameEvents.StateChanged -= OnStateChanged;

        void OnStateChanged(GameStateId state)
        {
            if (state == GameStateId.Play) timer = 1f;
        }

        void Update()
        {
            var game = GameManager.Instance;
            if (game == null || game.CurrentState != GameStateId.Play || !placement.HasPlaced) return;

            timer -= Time.deltaTime;
            if (timer > 0f) return;
            timer = game.Settings.spawnInterval;

            alive.RemoveAll(e => e == null);
            if (alive.Count >= game.Settings.maxEnemiesAlive) return;

            var type = Random.value < meleeChance ? EnemyType.Melee : EnemyType.Shooter;
            alive.Add(factory.Create(type, RandomSpawnPoint(), player, game.Settings));
            GameEvents.RaiseSound(SoundCue.EnemySpawn);
        }

        Vector3 RandomSpawnPoint()
        {
            Vector2 ring = Random.insideUnitCircle.normalized * spawnRadius;
            return placement.PlacedWorld.position + new Vector3(ring.x, 0f, ring.y);
        }
    }
}
