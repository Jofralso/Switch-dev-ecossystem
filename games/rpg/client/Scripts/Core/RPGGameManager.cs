using Framework.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.RPG.Core
{
    public enum GameState
    {
        MainMenu,
        CharacterSelect,
        Playing,
        Paused,
        Inventory,
        Dialogue
    }

    public class RPGGameManager : MonoBehaviour
    {
        public static RPGGameManager Instance { get; private set; }

        [Header("Configuration")]
        public int maxPartySize = 4;
        public GameObject playerPrefab;

        [Header("State")]
        public GameState state = GameState.MainMenu;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            LoadMainMenu();
        }

        public void LoadMainMenu()
        {
            state = GameState.MainMenu;
            SceneManager.LoadScene("MainMenu");
        }

        public void StartNewGame()
        {
            state = GameState.Playing;
            SceneManager.LoadScene("World_01");
        }

        public void OpenInventory()
        {
            state = GameState.Inventory;
            // TODO: Open inventory UI
        }

        public void ShowDialogue(string dialogueText)
        {
            state = GameState.Dialogue;
            // TODO: Show dialogue UI
        }
    }
}