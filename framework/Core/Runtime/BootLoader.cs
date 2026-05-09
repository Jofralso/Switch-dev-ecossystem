using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework.Core
{
    public class BootLoader : MonoBehaviour
    {
        [Header("Boot Configuration")]
        public string mainMenuScene = "MainMenu";
        public bool autoLoadMenu = true;

        private void Start()
        {
            if (autoLoadMenu)
            {
                SceneManager.LoadScene(mainMenuScene);
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            var boot = new GameObject("BootLoader");
            boot.AddComponent<BootLoader>();
            DontDestroyOnLoad(boot);

            // These will be replaced by game-specific implementations
            // Games should override these in their own BootLoader or use ServiceLocator
            var input = new GameObject("InputManager");
            input.AddComponent<Framework.Input.InputManager>();
            DontDestroyOnLoad(input);

            var audio = new GameObject("AudioManager");
            audio.AddComponent<Framework.Audio.AudioManager>();
            DontDestroyOnLoad(audio);

            var perf = new GameObject("PerformanceMonitor");
            perf.AddComponent<Framework.Core.PerformanceMonitor>();
            DontDestroyOnLoad(perf);

            var resolution = new GameObject("DynamicResolution");
            resolution.AddComponent<Framework.Platform.DynamicResolution>();
            DontDestroyOnLoad(resolution);
        }
    }
}