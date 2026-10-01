# Survival AR Shooter: Technical Documentation

## 1. Architecture overview
The game is a set of small managers that talk through a static event hub (`GameEvents`), so no system holds a direct reference to the UI.

```
              +-----------------+
   UI input ->|   GameManager   |<-- ARPlacementController (world placed)
              | (state machine) |<-- PlayerHealth (died)
              +--------+--------+
                       | StateChanged / ScoreChanged / TimeChanged
     +-----------------+------------------+-----------------+
     v                 v                  v                 v
  UIManager        EnemySpawner       AudioManager     LeaderboardService
 (panels, HUD)   (EnemyFactory,      (SoundRequested)   (PlayerPrefs)
                  ProjectilePool)
```

Flow: **Menu -> Placement -> Play -> End**. `GameManager` owns the current state, score, kills and the session timer. Difficulty values live in `DifficultySettings` ScriptableObjects.

## 2. OOP structure
- **Abstraction / inheritance:** `EnemyBase` (abstract) holds health, hit feedback, movement helpers and death. `MeleeEnemy` and `ShooterEnemy` implement `Act()`.
- **Polymorphism:** projectiles and enemies treat targets as `IDamageable`. Player and enemies share the interface; a bullet damages any opposing `Team`.
- **Encapsulation:** state is private or `SerializeField`; other classes use properties (`CurrentState`, `Score`) and methods (`TakeDamage`).

| Enemy | Role | Health | Behaviour |
|---|---|---|---|
| Melee (demon) | chaser | 2 hits to kill | walks to the player, damages only inside a short range, attack cooldown |
| Shooter (alien) | ranged | 5 hits to kill | stops at its shoot distance, fires pooled projectiles |

## 3. Design patterns
| Pattern | Where | Why |
|---|---|---|
| Singleton | `GameManager`, `AudioManager` | one owner of game flow and audio |
| State | `GameState` and `MenuState`, `PlacementState`, `PlayState`, `EndState`, `GameStateMachine` | each phase has its own enter, tick and exit logic |
| Factory | `EnemyFactory.Create(type, ...)` | spawner does not know prefabs or wiring |
| Observer | `GameEvents` | UI, audio and leaderboard react without coupling |
| Object Pool | `ObjectPool<T>`, `ProjectilePool` | no allocation during combat |

## 4. Object pool
`ProjectilePool` pre-instantiates a fixed number of `Projectile` objects at startup (30 player, 20 enemy). `Fire()` takes an inactive bullet, calls `Projectile.Fire(...)` to reset position, direction, damage, team and age, and activates it. When a bullet hits something or its lifetime ends it calls back into the pool, which runs `ResetState()` and deactivates it. There is no `Instantiate` or `Destroy` for bullets during gameplay. The player and the Shooter enemy use the same pool type, and the `Team` field decides who a bullet can hurt.

## 5. Sound system
One `AudioManager` GameObject holds two `AudioSource` components: one looping for music, one for effects played with `PlayOneShot`, so overlapping effects need no extra components. Gameplay code only raises `GameEvents.RaiseSound(SoundCue)`; the manager maps the cue to a clip.

| Cue | Raised by |
|---|---|
| PlayerShoot | `PlayerShooter` |
| PlayerDeath | `PlayerHealth` |
| EnemySpawn | `EnemySpawner` |
| EnemyShoot | `ShooterEnemy` |
| MeleeHit | `MeleeEnemy` attack |
| UiClick | `PauseUI` |

Music pauses with the pause menu.

## 6. AR system
`ARPlaneManager` detects horizontal planes and shows a custom prefab (`CustomPlane`): an `ARPlaneMeshVisualizer` mesh with a tiled texture that carries the author's name. `ARPlacementController` raycasts a tap against detected planes, places the arena once, attaches it to an `ARAnchor`, then disables plane detection and hides the planes.

## 7. Leaderboard
`LeaderboardService` stores a JSON list in `PlayerPrefs`, newest first, trimmed to 5 entries. Each session saves score, enemies defeated, time survived, difficulty and date when the game ends.
