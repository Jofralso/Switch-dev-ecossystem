using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core
{
    public class PerformanceMonitor : MonoBehaviour
    {
        [Header("Monitoring")]
        public bool enabled = true;
        public float sampleInterval = 1f;
        public int maxSamples = 300;

        [Header("Thresholds (Switch)")]
        public float frameTimeWarning = 0.033f;
        public float frameTimeCritical = 0.05f;
        public int drawCallWarning = 80;
        public int drawCallCritical = 120;

        public struct FrameSample
        {
            public float time;
            public float frameTimeMs;
            public int drawCalls;
            public int triangleCount;
            public long allocatedMemory;
        }

        private readonly List<FrameSample> _samples = new();
        private float _sampleTimer;
        private int _lastDrawCalls;
        private int _lastTriangles;

        private void Update()
        {
            if (!enabled) return;

            _sampleTimer += Time.deltaTime;
            if (_sampleTimer >= sampleInterval)
            {
                _sampleTimer = 0f;
                SampleFrame();
            }
        }

        private void SampleFrame()
        {
            var sample = new FrameSample
            {
                time = Time.time,
                frameTimeMs = Time.deltaTime * 1000f,
                drawCalls = GetDrawCalls(),
                triangleCount = GetTriangles(),
                allocatedMemory = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong() / (1024 * 1024)
            };

            _samples.Add(sample);
            if (_samples.Count > maxSamples)
                _samples.RemoveAt(0);

            CheckThresholds(sample);
        }

        private static int GetDrawCalls()
        {
            return UnityEngine.Profiling.Profiler.GetAllocatedMemoryForGraphicsDriver();
        }

        private static int GetTriangles()
        {
            return 0;
        }

        private void CheckThresholds(FrameSample sample)
        {
            if (sample.frameTimeMs > frameTimeCritical * 1000f)
                Debug.LogWarning($"[PERF] Critical frame time: {sample.frameTimeMs:F1}ms");

            if (sample.drawCalls > drawCallCritical)
                Debug.LogWarning($"[PERF] Critical draw calls: {sample.drawCalls}");
        }

        public IReadOnlyList<FrameSample> GetSamples() => _samples;

        public FrameSample GetAverage()
        {
            if (_samples.Count == 0) return default;

            float avgFrame = 0f, avgDraws = 0f;
            long avgMem = 0;

            foreach (var s in _samples)
            {
                avgFrame += s.frameTimeMs;
                avgDraws += s.drawCalls;
                avgMem += s.allocatedMemory;
            }

            int count = _samples.Count;
            return new FrameSample
            {
                frameTimeMs = avgFrame / count,
                drawCalls = (int)(avgDraws / count),
                allocatedMemory = avgMem / count
            };
        }

        public string GenerateReport()
        {
            var avg = GetAverage();
            return $"Avg Frame: {avg.frameTimeMs:F1}ms | Avg Draws: {avg.drawCalls} | " +
                   $"Memory: {avg.allocatedMemory}MB | Samples: {_samples.Count}";
        }

        private void OnGUI()
        {
            if (!enabled) return;
            var avg = GetAverage();
            GUILayout.Label($"Frame: {avg.frameTimeMs:F1}ms | Draws: {avg.drawCalls} | Mem: {avg.allocatedMemory}MB");
        }
    }
}