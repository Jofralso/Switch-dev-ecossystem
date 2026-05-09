using Game.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TutorialPrompt : MonoBehaviour
    {
        [Header("Prompt Settings")]
        public string message = "Press SPACE to jump!";
        public float displayDuration = 3f;
        public float fadeDuration = 0.5f;
        public bool dismissOnInteract = true;

        [Header("UI")]
        public Text promptText;
        public CanvasGroup canvasGroup;

        private float _timer;
        private bool _isDismissed;

        private enum FadeState { FadingIn, Showing, FadingOut, Hidden }
        private FadeState _fadeState = FadeState.Hidden;

        private void Start()
        {
            if (promptText != null) promptText.text = message;
            if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                _fadeState = FadeState.FadingIn;
                _timer = fadeDuration;
            }
        }

        private void Update()
        {
            if (_isDismissed) return;

            if (dismissOnInteract && Input.anyKeyDown)
            {
                Dismiss();
                return;
            }

            switch (_fadeState)
            {
                case FadeState.FadingIn:
                    _timer -= Time.deltaTime;
                    if (canvasGroup != null)
                        canvasGroup.alpha = 1f - (_timer / fadeDuration);
                    if (_timer <= 0f)
                    {
                        canvasGroup.alpha = 1f;
                        _fadeState = FadeState.Showing;
                        _timer = displayDuration;
                    }
                    break;

                case FadeState.Showing:
                    _timer -= Time.deltaTime;
                    if (_timer <= 0f)
                    {
                        _fadeState = FadeState.FadingOut;
                        _timer = fadeDuration;
                    }
                    break;

                case FadeState.FadingOut:
                    _timer -= Time.deltaTime;
                    if (canvasGroup != null)
                        canvasGroup.alpha = _timer / fadeDuration;
                    if (_timer <= 0f)
                    {
                        canvasGroup.alpha = 0f;
                        _fadeState = FadeState.Hidden;
                        gameObject.SetActive(false);
                    }
                    break;
            }
        }

        public void Dismiss()
        {
            _isDismissed = true;
            if (canvasGroup != null)
                canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

        public void Show(string newMessage, float duration)
        {
            message = newMessage;
            displayDuration = duration;
            _isDismissed = false;
            _fadeState = FadeState.FadingIn;
            _timer = fadeDuration;

            if (promptText != null)
                promptText.text = message;

            gameObject.SetActive(true);
        }
    }
}
