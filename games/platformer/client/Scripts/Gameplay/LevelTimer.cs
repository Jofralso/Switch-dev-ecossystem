using Game.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Gameplay
{
    public class LevelTimer : MonoBehaviour
    {
        [Header("Timer Mode")]
        public bool countDown = true;
        public float startTime = 60f;
        public bool failOnTimeout = true;

        [Header("UI")]
        public Text timerDisplay;
        public Color normalColor = Color.white;
        public Color warningColor = Color.yellow;
        public Color dangerColor = Color.red;
        public float warningThreshold = 0.3f;
        public float dangerThreshold = 0.1f;

        [Header("Events")]
        public UnityEngine.Events.UnityEvent onTimeUp;

        private float _currentTime;
        private bool _isRunning;
        private bool _timeUpTriggered;

        private void Start()
        {
            _currentTime = startTime;
            _isRunning = true;
        }

        private void Update()
        {
            if (!_isRunning) return;
            if (GameManager.Instance == null || GameManager.Instance.state != GameState.Playing) return;

            if (countDown)
            {
                _currentTime -= Time.deltaTime;
                if (_currentTime <= 0f)
                {
                    _currentTime = 0f;
                    if (!_timeUpTriggered)
                    {
                        _timeUpTriggered = true;
                        onTimeUp?.Invoke();
                        if (failOnTimeout)
                        {
                            GameManager.Instance.ReloadLevel();
                        }
                    }
                }
            }
            else
            {
                _currentTime += Time.deltaTime;
            }

            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (timerDisplay == null) return;

            int minutes = Mathf.FloorToInt(_currentTime / 60f);
            int seconds = Mathf.FloorToInt(_currentTime % 60f);
            timerDisplay.text = $"{minutes:0}:{seconds:00}";

            if (countDown)
            {
                float ratio = _currentTime / startTime;
                if (ratio < dangerThreshold)
                    timerDisplay.color = dangerColor;
                else if (ratio < warningThreshold)
                    timerDisplay.color = warningColor;
                else
                    timerDisplay.color = normalColor;
            }
        }

        public void Pause() => _isRunning = false;
        public void Resume() => _isRunning = true;
        public float GetTime() => _currentTime;
        public float GetProgress() => countDown ? _currentTime / startTime : _currentTime / startTime;
    }
}
