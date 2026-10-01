using SurvivalShooter.Audio;
using SurvivalShooter.Core;
using SurvivalShooter.Leaderboard;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalShooter.UI
{
    /// <summary>Start menu, leaderboard and settings wiring.</summary>
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] Button startButton;
        [SerializeField] Button leaderboardButton;
        [SerializeField] Button settingsButton;
        [SerializeField] Button quitButton;
        [SerializeField] Button leaderboardBackButton;
        [SerializeField] Button settingsBackButton;
        [SerializeField] Button[] difficultyButtons;
        [SerializeField] Button musicButton;
        [SerializeField] Button sfxButton;
        [SerializeField] GameObject leaderboardPanel;
        [SerializeField] GameObject settingsPanel;
        [SerializeField] TextMeshProUGUI[] leaderboardRows;

        static readonly Color Idle = new Color(0.77f, 0.78f, 0.78f, 1f);
        static readonly Color Selected = new Color(0.78f, 0.16f, 0.14f, 1f);

        void Awake()
        {
            startButton.onClick.AddListener(() => GameManager.Instance.StartGame());
            leaderboardButton.onClick.AddListener(ShowLeaderboard);
            settingsButton.onClick.AddListener(() => settingsPanel.SetActive(true));
            quitButton.onClick.AddListener(Application.Quit);
            leaderboardBackButton.onClick.AddListener(() => leaderboardPanel.SetActive(false));
            settingsBackButton.onClick.AddListener(() => settingsPanel.SetActive(false));
            musicButton.onClick.AddListener(ToggleMusic);
            sfxButton.onClick.AddListener(ToggleSfx);

            for (int i = 0; i < difficultyButtons.Length; i++)
            {
                var level = (Difficulty)i;
                difficultyButtons[i].onClick.AddListener(() => SelectDifficulty(level));
            }
        }

        void Start()
        {
            SelectDifficulty(Difficulty.Normal);
            RefreshAudioLabels();
        }

        void ShowLeaderboard()
        {
            var entries = LeaderboardService.Load();
            for (int i = 0; i < leaderboardRows.Length; i++)
            {
                if (i >= entries.Count) { leaderboardRows[i].text = $"{i + 1}.  ---"; continue; }
                var e = entries[i];
                leaderboardRows[i].text = $"{i + 1}.  {e.score} pts   {e.defeated} kills   {HudUI.FormatTime(e.secondsSurvived)}   {e.difficulty}   {e.date}";
            }
            leaderboardPanel.SetActive(true);
        }

        void SelectDifficulty(Difficulty level)
        {
            GameManager.Instance.SetDifficulty(level);
            for (int i = 0; i < difficultyButtons.Length; i++)
                difficultyButtons[i].targetGraphic.color = i == (int)level ? Selected : Idle;
        }

        void ToggleMusic()
        {
            var audio = AudioManager.Instance;
            if (audio == null) return;
            audio.MusicOn = !audio.MusicOn;
            RefreshAudioLabels();
        }

        void ToggleSfx()
        {
            var audio = AudioManager.Instance;
            if (audio == null) return;
            audio.SfxOn = !audio.SfxOn;
            RefreshAudioLabels();
        }

        void RefreshAudioLabels()
        {
            var audio = AudioManager.Instance;
            if (audio == null) return;
            SetLabel(musicButton, "MUSIC  " + (audio.MusicOn ? "ON" : "OFF"));
            SetLabel(sfxButton, "SOUND FX  " + (audio.SfxOn ? "ON" : "OFF"));
        }

        static void SetLabel(Button button, string text) =>
            button.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }
}
