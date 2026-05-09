using Game.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GameHUD : MonoBehaviour
    {
        [Header("UI References")]
        public Text timerText;
        public Text levelText;
        public Text keyText;
        public Text deathText;
        public GameObject pausePanel;
        public GameObject levelCompletePanel;
        public Text completeTimeText;
        public Text completeDeathsText;

        private void Start()
        {
            if (pausePanel != null) pausePanel.SetActive(false);
            if (levelCompletePanel != null) levelCompletePanel.SetActive(false);
        }

        private void Update()
        {
            var gm = GameManager.Instance;
            if (gm == null) return;

            switch (gm.state)
            {
                case GameState.Playing:
                    UpdateHUD(gm);
                    if (pausePanel != null) pausePanel.SetActive(false);
                    if (levelCompletePanel != null) levelCompletePanel.SetActive(false);
                    break;

                case GameState.Paused:
                    if (pausePanel != null) pausePanel.SetActive(true);
                    break;

                case GameState.LevelComplete:
                    if (levelCompletePanel != null)
                    {
                        levelCompletePanel.SetActive(true);
                        if (completeTimeText != null)
                            completeTimeText.text = $"Time: {gm.GetLevelTime():F1}s";
                        if (completeDeathsText != null)
                            completeDeathsText.text = $"Deaths: {gm.GetTotalDeaths()}";
                    }
                    break;
            }
        }

        private void UpdateHUD(GameManager gm)
        {
            if (timerText != null)
                timerText.text = $"{gm.GetLevelTime():F1}s";

            if (levelText != null)
                levelText.text = $"Level {gm.GetCurrentLevel()}";

            var levelManager = FindObjectOfType<LevelManager>();
            if (levelManager != null && keyText != null)
                keyText.text = $"Keys: {levelManager.GetKeysCollected()}";

            if (deathText != null)
                deathText.text = $"Deaths: {gm.GetTotalDeaths()}";
        }

        public void OnResumeClicked()
        {
            GameManager.Instance?.TogglePause();
        }

        public void OnRestartClicked()
        {
            GameManager.Instance?.ReloadLevel();
        }

        public void OnNextLevelClicked()
        {
            GameManager.Instance?.NextLevel();
        }

        public void OnMenuClicked()
        {
            GameManager.Instance?.BackToMenu();
        }
    }
}
