using SurvivalShooter.Audio;
using SurvivalShooter.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalShooter.UI
{
    /// <summary>Pause button and panel.</summary>
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] Button pauseButton;
        [SerializeField] GameObject pausePanel;
        [SerializeField] Button resumeButton;
        [SerializeField] Button menuButton;

        void Awake()
        {
            pauseButton.onClick.AddListener(() => SetPaused(true));
            resumeButton.onClick.AddListener(() => SetPaused(false));
            menuButton.onClick.AddListener(() =>
            {
                SetPaused(false);
                GameManager.Instance.ReturnToMenu();
            });
        }

        void OnEnable() => GameEvents.StateChanged += OnStateChanged;

        void OnDisable()
        {
            GameEvents.StateChanged -= OnStateChanged;
            Time.timeScale = 1f;
        }

        // Never leave the game frozen across states.
        void OnStateChanged(GameStateId state)
        {
            Time.timeScale = 1f;
            if (pausePanel != null) pausePanel.SetActive(false);
        }

        void SetPaused(bool paused)
        {
            Time.timeScale = paused ? 0f : 1f;
            pausePanel.SetActive(paused);
            if (AudioManager.Instance != null) AudioManager.Instance.SetMusicPaused(paused);
            GameEvents.RaiseSound(SoundCue.UiClick);
        }
    }
}
