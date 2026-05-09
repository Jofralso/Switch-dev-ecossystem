using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        LevelComplete,
        GameOver
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Configuration")]
        public int activePlayerCount = 4;
        public GameObject playerPrefab;
        public List<LevelData> levels = new();

        [Header("State")]
        public GameState state = GameState.MainMenu;

        private readonly List<GameObject> _players = new();
        private LevelProgress _progress;
        private float _levelStartTime;
        private int _currentLevelIndex;
        private int _totalDeaths;

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
            LoadProgress();
        }

        private void Update()
        {
            if (state == GameState.Playing)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                    TogglePause();

                CheckAllPlayersAtExit();
            }
        }

        public void StartGame()
        {
            _currentLevelIndex = 0;
            _totalDeaths = 0;
            LoadLevel(_currentLevelIndex);
        }

        public void LoadLevel(int index)
        {
            if (index < 0 || index >= levels.Count)
            {
                BackToMenu();
                return;
            }

            _currentLevelIndex = index;
            var levelData = levels[index];
            _levelStartTime = Time.time;

            if (!string.IsNullOrEmpty(levelData.scene.scenePath))
                SceneManager.LoadScene(levelData.scene.scenePath);
            else
                SceneManager.LoadScene("Level_" + levelData.levelNumber);

            state = GameState.Playing;
        }

        public void ReloadLevel()
        {
            _totalDeaths = 0;
            LoadLevel(_currentLevelIndex);
        }

        public void NextLevel()
        {
            float time = Time.time - _levelStartTime;
            _progress.MarkCompleted(_currentLevelIndex + 1, time, _totalDeaths);
            SaveProgress();
            LoadLevel(_currentLevelIndex + 1);
        }

        public void TogglePause()
        {
            state = state == GameState.Playing ? GameState.Paused : GameState.Playing;
            Time.timeScale = state == GameState.Paused ? 0f : 1f;
        }

        public void BackToMenu()
        {
            Time.timeScale = 1f;
            state = GameState.MainMenu;
            SceneManager.LoadScene("MainMenu");
        }

        public void RegisterPlayer(GameObject player)
        {
            if (!_players.Contains(player))
                _players.Add(player);
        }

        public void UnregisterPlayer(GameObject player)
        {
            _players.Remove(player);
        }

        public void RegisterDeath()
        {
            _totalDeaths++;
        }

        public IReadOnlyList<GameObject> GetPlayers() => _players;

        public float GetLevelTime() => Time.time - _levelStartTime;

        public int GetCurrentLevel() => _currentLevelIndex + 1;

        public int GetTotalDeaths() => _totalDeaths;

        private void CheckAllPlayersAtExit()
        {
            if (_players.Count == 0) return;
            bool allAtExit = true;
            foreach (var p in _players)
            {
                if (p == null) continue;
                var playerController = p.GetComponent<Player.PlayerController>();
                if (playerController != null && !playerController.IsAtExit())
                {
                    allAtExit = false;
                    break;
                }
            }

            if (allAtExit)
            {
                state = GameState.LevelComplete;
            }
        }

        private void LoadProgress()
        {
            string json = PlayerPrefs.GetString("GameProgress", "");
            if (!string.IsNullOrEmpty(json))
            {
                _progress = JsonUtility.FromJson<LevelProgress>(json);
            }
            _progress ??= new LevelProgress();
        }

        private void SaveProgress()
        {
            string json = JsonUtility.ToJson(_progress);
            PlayerPrefs.SetString("GameProgress", json);
            PlayerPrefs.Save();
        }

        public int GetHighestUnlockedLevel() => _progress?.highestLevelUnlocked ?? 1;
        public List<LevelResult> GetLevelResults() => _progress?.results ?? new List<LevelResult>();
    }
}
