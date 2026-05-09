using Framework.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Shooter.Core
{
    public enum GameState
    {
        MainMenu,
        Lobby,
        Playing,
        Paused,
        GameOver,
        Scoreboard
    }

    public class ShooterGameManager : MonoBehaviour
    {
        public static ShooterGameManager Instance { get; private set; }

        [Header("Configuration")]
        public int maxPlayers = 16;
        public GameObject playerPrefab;
        public List<LevelData> maps = new();

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

        public void StartMatch(string mapName)
        {
            state = GameState.Playing;
            // TODO: Load map
        }

        public void EndMatch()
        {
            state = GameState.Scoreboard;
            // TODO: Show scoreboard
        }
    }

    [System.Serializable]
    public class LevelData
    {
        public string mapName;
        public string scenePath;
        public string thumbnail;
        public int maxPlayers;
        public string gameMode;
    }
}