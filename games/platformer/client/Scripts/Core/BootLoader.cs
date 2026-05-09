using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core
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

            var input = new GameObject("InputManager");
            input.AddComponent<InputManager>();
            DontDestroyOnLoad(input);

            var gm = new GameObject("GameManager");
            gm.AddComponent<GameManager>();
            DontDestroyOnLoad(gm);

            var audio = new GameObject("AudioManager");
            audio.AddComponent<AudioManager>();
            DontDestroyOnLoad(audio);
        }
    }
}
