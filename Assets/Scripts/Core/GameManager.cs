using System;
using SurvivalShooter.Leaderboard;
using SurvivalShooter.States;
using UnityEngine;

namespace SurvivalShooter.Core
{
    /// <summary>Singleton game flow owner.</summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] DifficultySettings[] difficulties;
        [SerializeField] Difficulty selectedDifficulty = Difficulty.Normal;

        readonly GameStateMachine machine = new GameStateMachine();
        MenuState menu;
        PlacementState placement;
        PlayState play;
        EndState end;

        bool worldPlaced;

        public GameStateId CurrentState => machine.Current.Id;
        public DifficultySettings Settings =>difficulties[(int)selectedDifficulty];
        public int Score { get; private set; }
        public int EnemiesDefeated { get; private set; }
        public float TimeRemaining { get; private set; }
        public float TimeSurvived => Settings.sessionSeconds - TimeRemaining;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;

            menu = new MenuState(this);
            placement = new PlacementState(this);
            play = new PlayState(this);
            end = new EndState(this);
        }

        void OnEnable()
        {
            GameEvents.EnemyKilled += OnEnemyKilled;
            GameEvents.PlayerDied += EndGame;
            GameEvents.GameWorldPlaced += OnWorldPlaced;
        }

        void OnDisable()
        {
            GameEvents.EnemyKilled -= OnEnemyKilled;
            GameEvents.PlayerDied -= EndGame;
            GameEvents.GameWorldPlaced -= OnWorldPlaced;
        }

        void Start() => machine.ChangeState(menu);

        void Update() => machine.Tick(Time.deltaTime);

        // UI flow API
        public void SetDifficulty(Difficulty level) => selectedDifficulty = level;

        /// <summary>Placement, or straight to play.</summary>
        public void StartGame()
        {
            if (worldPlaced) machine.ChangeState(play);
            else machine.ChangeState(placement);
        }

        public void RestartGame() => machine.ChangeState(play);

        public void ReturnToMenu() => machine.ChangeState(menu);

        public void EndGame()
        {
            if (CurrentState != GameStateId.Play) return;

            LeaderboardService.Add(new ScoreEntry
            {
                score = Score,
                defeated = EnemiesDefeated,
                secondsSurvived = TimeSurvived,
                difficulty = Settings.level.ToString(),
                date = DateTime.Now.ToString("dd MMM HH:mm"),
            });
            machine.ChangeState(end);
        }

        // PlayState hooks

        public void BeginSession()
        {
            Score = 0;
            EnemiesDefeated = 0;
            TimeRemaining = Settings.sessionSeconds;
            GameEvents.RaiseScoreChanged(Score);
            GameEvents.RaiseTimeRemainingChanged(TimeRemaining);
        }

        public void TickSession(float deltaTime)
        {
            TimeRemaining = Mathf.Max(0f, TimeRemaining - deltaTime);
            GameEvents.RaiseTimeRemainingChanged(TimeRemaining);
            if (TimeRemaining <= 0f) EndGame();
        }

        // Event handlers

        void OnWorldPlaced()
        {
            worldPlaced = true;
            if (CurrentState == GameStateId.Placement) machine.ChangeState(play);
        }

        void OnEnemyKilled(int scoreValue)
        {
            if (CurrentState != GameStateId.Play) return;
            Score += scoreValue;
            EnemiesDefeated++;
            GameEvents.RaiseScoreChanged(Score);
        }
    }
}
