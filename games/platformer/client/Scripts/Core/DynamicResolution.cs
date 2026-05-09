using UnityEngine;

namespace Game.Core
{
    public class DynamicResolution : MonoBehaviour
    {
        [Header("Resolution Scaling")]
        public Vector2Int baseResolution = new(1920, 1080);
        public Vector2Int minResolution = new(1280, 720);
        public float targetFrameTime = 0.033f;
        public float scaleStep = 0.05f;
        public float adjustmentInterval = 1f;

        [Header("Switch-Safe Max")]
        public int maxDrawCalls = 100;
        public int maxVertices = 500000;

        private float _timer;
        private float _currentScale = 1f;
        private float _smoothedFrameTime;

        private void Start()
        {
            _currentScale = 1f;
            ApplyResolution();
        }

        private void Update()
        {
            _smoothedFrameTime = Mathf.Lerp(_smoothedFrameTime, Time.deltaTime, 0.1f);
            _timer += Time.deltaTime;

            if (_timer >= adjustmentInterval)
            {
                _timer = 0f;
                AdjustResolution();
            }
        }

        private void AdjustResolution()
        {
            if (_smoothedFrameTime > targetFrameTime * 1.1f)
            {
                _currentScale = Mathf.Max(_currentScale - scaleStep,
                    (float)minResolution.x / baseResolution.x);
                ApplyResolution();
            }
            else if (_smoothedFrameTime < targetFrameTime * 0.8f && _currentScale < 1f)
            {
                _currentScale = Mathf.Min(_currentScale + scaleStep, 1f);
                ApplyResolution();
            }
        }

        private void ApplyResolution()
        {
            int width = Mathf.RoundToInt(baseResolution.x * _currentScale);
            int height = Mathf.RoundToInt(baseResolution.y * _currentScale);

            width = Mathf.Max(width, minResolution.x);
            height = Mathf.Max(height, minResolution.y);

            Screen.SetResolution(width, height, Screen.fullScreen);
        }

        public float GetCurrentScale() => _currentScale;
    }
}
