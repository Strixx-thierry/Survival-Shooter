# Survival AR Shooter

A mobile AR survival shooter built with Unity 6 and AR Foundation. Scan a floor, tap to place the arena, then survive the timer by shooting Melee and Shooter enemies. Built for Android.

## How to play
1. Pick a difficulty (Easy / Normal / Hard) and press **START**.
2. Move your phone slowly until the textured plane tracker appears, then tap it once to place the arena.
3. Aim with the phone, hold **FIRE** to shoot. Survive until the timer ends.
4. The end screen shows score, enemies defeated and time survived. The latest 5 sessions are saved in **LEADERBOARD**.

The game runs in landscape. The **II** button pauses.

## Features
- AR Foundation horizontal plane detection with a custom textured plane tracker showing the author's name
- Tap-to-place, once only, anchored; plane detection stops after placement
- Melee enemy (close range, attack cooldown) and Shooter enemy (keeps distance, fires projectiles), different models and health
- Object pooling for all projectiles (player and enemy)
- State machine game flow: Menu, Placement, Play, End
- Local leaderboard (PlayerPrefs JSON, latest 5 sessions)
- Sound for shoot, death, spawn, enemy shoot, melee hit, UI, plus looping music
- Difficulty levels driven by `DifficultySettings` ScriptableObjects

The whole game is one scene (`Assets/Scenes/Game.unity`). Menu, placement hint, HUD and end screen are UI panels switched by the state machine, so the menu runs over the live AR camera feed.

## Project layout
```
Assets/
  Scripts/Core         GameManager, GameEvents, DifficultySettings
  Scripts/States       State pattern (Menu, Placement, Play, End)
  Scripts/AR           ARPlacementController
  Scripts/Player       PlayerHealth, PlayerShooter
  Scripts/Enemies      EnemyBase, MeleeEnemy, ShooterEnemy, EnemyFactory, EnemySpawner
  Scripts/Pooling      ObjectPool<T>, ProjectilePool, Projectile
  Scripts/Audio        AudioManager
  Scripts/Leaderboard  LeaderboardService
  Scripts/UI           UIManager, MainMenuUI, HudUI, EndUI, PauseUI
Tools/make_plane_texture.py   regenerates the plane tracker texture with a name
docs/TechnicalDocumentation.md
```

## Build
Unity 6000.6.0f1, Android build target.
1. Open the project in Unity.
2. Open `Assets/Scenes/Game.unity`.
3. File > Build Profiles > Android > Build.

Change the name on the plane tracker: `python Tools/make_plane_texture.py "Your Full Name"`.

## Assets
Enemy models: Cute Animated Monsters pack. Font: Jaro (SIL OFL). Placeholder sound effects and music are generated procedurally.
