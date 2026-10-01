using SurvivalShooter.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalShooter.UI
{
    /// <summary>Live health, score, time.</summary>
    public class HudUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI timeText;
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] TextMeshProUGUI healthText;
        [SerializeField] RectTransform healthFill;
        [SerializeField] Image damageFlash;

        int lastHealth = -1;
        float flash;

        public static string FormatTime(float seconds)
        {
            int total = Mathf.CeilToInt(seconds);
            return $"{total / 60:00}:{total % 60:00}";
        }

        void OnEnable()
        {
            GameEvents.PlayerHealthChanged += OnHealth;
            GameEvents.ScoreChanged += OnScore;
            GameEvents.TimeRemainingChanged += OnTime;
            GameEvents.StateChanged += OnState;
        }

        void OnDisable()
        {
            GameEvents.PlayerHealthChanged -= OnHealth;
            GameEvents.ScoreChanged -= OnScore;
            GameEvents.TimeRemainingChanged -= OnTime;
            GameEvents.StateChanged -= OnState;
        }

        void OnState(GameStateId state)
        {
            if (state != GameStateId.Play) return;
            lastHealth = -1;
            flash = 0f;
        }

        void OnHealth(int current, int max)
        {
            healthText.text = $"{current}/{max}";
            healthFill.anchorMax = new Vector2(Mathf.Clamp01((float)current / max), 1f);
            if (lastHealth >= 0 && current < lastHealth) flash = 0.4f;
            lastHealth = current;
        }

        void OnScore(int score) => scoreText.text = $"SCORE {score}";

        void OnTime(float seconds) => timeText.text = FormatTime(seconds);

        void Update()
        {
            if (flash <= 0f) return;
            flash -= Time.deltaTime * 1.5f;
            var color = damageFlash.color;
            color.a = Mathf.Max(0f, flash);
            damageFlash.color = color;
        }
    }
}
