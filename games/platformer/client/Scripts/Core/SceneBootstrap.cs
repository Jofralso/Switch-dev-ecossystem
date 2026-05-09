using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core
{
    public class SceneBootstrap : MonoBehaviour
    {
        [Header("Auto-Setup")]
        public bool spawnCamera = true;
        public bool spawnInputManager = true;
        public bool spawnGameManager = true;

        private void Awake()
        {
            if (spawnInputManager && InputManager.Instance == null)
            {
                var obj = new GameObject("InputManager");
                obj.AddComponent<InputManager>();
            }

            if (spawnGameManager && GameManager.Instance == null)
            {
                var obj = new GameObject("GameManager");
                obj.AddComponent<GameManager>();
            }

            if (spawnCamera && Camera.main == null)
            {
                var obj = new GameObject("MainCamera");
                var cam = obj.AddComponent<Camera>();
                cam.orthographic = true;
                cam.orthographicSize = 10f;
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = Color.black;
                obj.AddComponent<CameraController>();
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (GameManager.Instance != null && GameManager.Instance.state == GameState.Playing)
            {
                if (FindObjectOfType<LevelManager>() == null && scene.name != "MainMenu")
                {
                    var obj = new GameObject("LevelManager");
                    obj.AddComponent<LevelManager>();
                }
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
