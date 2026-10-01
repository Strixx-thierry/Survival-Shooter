using SurvivalShooter.Core;
using UnityEngine;

namespace SurvivalShooter.UI
{
    /// <summary>Shows panels per game state.</summary>
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject menuPanel;
        [SerializeField] GameObject placementHint;
        [SerializeField] GameObject hudPanel;
        [SerializeField] GameObject endPanel;
        [SerializeField] EndUI endUI;

        void OnEnable() => GameEvents.StateChanged += OnStateChanged;
        void OnDisable() => GameEvents.StateChanged -= OnStateChanged;

        void OnStateChanged(GameStateId state)
        {
            menuPanel.SetActive(state == GameStateId.Menu);
            placementHint.SetActive(state == GameStateId.Placement);
            hudPanel.SetActive(state == GameStateId.Play);
            endPanel.SetActive(state == GameStateId.End);
            if (state == GameStateId.End) endUI.Show();
        }
    }
}
