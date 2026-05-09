using Game.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Panels")]
        public GameObject mainPanel;
        public GameObject levelSelectPanel;
        public GameObject settingsPanel;
        public GameObject creditsPanel;

        [Header("Player Count")]
        public Text playerCountText;
        public int minPlayers = 1;
        public int maxPlayers = 4;

        private void Start()
        {
            ShowMainPanel();
        }

        public void ShowMainPanel()
        {
            SetActivePanel(mainPanel);
        }

        public void ShowLevelSelect()
        {
            SetActivePanel(levelSelectPanel);
        }

        public void ShowSettings()
        {
            SetActivePanel(settingsPanel);
        }

        public void ShowCredits()
        {
            SetActivePanel(creditsPanel);
        }

        public void StartGame()
        {
            var gm = GameManager.Instance;
            if (gm == null) return;

            if (playerCountText != null)
            {
                int.TryParse(playerCountText.text, out int count);
                gm.activePlayerCount = Mathf.Clamp(count, minPlayers, maxPlayers);
            }

            gm.StartGame();
        }

        public void IncreasePlayers()
        {
            if (playerCountText == null) return;
            int count = int.Parse(playerCountText.text);
            count = Mathf.Min(count + 1, maxPlayers);
            playerCountText.text = count.ToString();
        }

        public void DecreasePlayers()
        {
            if (playerCountText == null) return;
            int count = int.Parse(playerCountText.text);
            count = Mathf.Max(count - 1, minPlayers);
            playerCountText.text = count.ToString();
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void SetActivePanel(GameObject panel)
        {
            if (mainPanel != null) mainPanel.SetActive(panel == mainPanel);
            if (levelSelectPanel != null) levelSelectPanel.SetActive(panel == levelSelectPanel);
            if (settingsPanel != null) settingsPanel.SetActive(panel == settingsPanel);
            if (creditsPanel != null) creditsPanel.SetActive(panel == creditsPanel);
        }
    }
}
