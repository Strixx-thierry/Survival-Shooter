using SurvivalShooter.Core;
using SurvivalShooter.Leaderboard;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalShooter.UI
{
    /// <summary>End screen summary and buttons.</summary>
    public class EndUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI titleText;
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] TextMeshProUGUI defeatedText;
        [SerializeField] TextMeshProUGUI timeText;
        [SerializeField] TextMeshProUGUI[] leaderboardRows;
        [SerializeField] Button restartButton;
        [SerializeField] Button menuButton;

        void Awake()
        {
            restartButton.onClick.AddListener(() => GameManager.Instance.RestartGame());
            menuButton.onClick.AddListener(() => GameManager.Instance.ReturnToMenu());
        }

        public void Show()
        {
            var game = GameManager.Instance;
            titleText.text = game.TimeRemaining <= 0f ? "YOU SURVIVED" : "GAME OVER";
            scoreText.text = $"Final Score: {game.Score}";
            defeatedText.text = $"Enemies Defeated: {game.EnemiesDefeated}";
            timeText.text = $"Time Survived: {HudUI.FormatTime(game.TimeSurvived)}";
            ShowLeaderboard();
        }

        void ShowLeaderboard()
        {
            var entries = LeaderboardService.Load();
            for (int i = 0; i < leaderboardRows.Length; i++)
            {
                if (i >= entries.Count) { leaderboardRows[i].text = $"{i + 1}.  ---"; continue; }
                var e = entries[i];
                leaderboardRows[i].text = $"{i + 1}.  {e.score} pts   {e.defeated} kills   {HudUI.FormatTime(e.secondsSurvived)}   {e.difficulty}";
            }
        }
    }
}
