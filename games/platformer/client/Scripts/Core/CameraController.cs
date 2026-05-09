using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class CameraController : MonoBehaviour
    {
        [Header("Follow Settings")]
        public Vector2 worldBoundsMin = new(-20f, -20f);
        public Vector2 worldBoundsMax = new(20f, 20f);
        public float smoothTime = 0.2f;
        public float zoomFactor = 2f;
        public float minZoom = 5f;
        public float maxZoom = 15f;
        public float zoomSmoothTime = 0.3f;

        private Camera _cam;
        private Vector3 _velocity;
        private float _zoomVelocity;

        private void Start()
        {
            _cam = GetComponent<Camera>();
            _cam ??= Camera.main;
        }

        private void LateUpdate()
        {
            var players = GameManager.Instance?.GetPlayers();
            if (players == null || players.Count == 0) return;

            Vector3 targetPos = CalculateCenter(players);
            float targetZoom = CalculateZoom(players);

            transform.position = Vector3.SmoothDamp(
                transform.position, targetPos, ref _velocity, smoothTime);

            if (_cam != null && _cam.orthographic)
            {
                _cam.orthographicSize = Mathf.SmoothDamp(
                    _cam.orthographicSize, targetZoom, ref _zoomVelocity, zoomSmoothTime);
            }
        }

        private Vector3 CalculateCenter(IReadOnlyList<GameObject> players)
        {
            Vector3 sum = Vector3.zero;
            int count = 0;

            foreach (var p in players)
            {
                if (p == null) continue;
                sum += p.transform.position;
                count++;
            }

            if (count == 0) return transform.position;

            Vector3 center = sum / count;
            center.z = transform.position.z;

            center.x = Mathf.Clamp(center.x, worldBoundsMin.x, worldBoundsMax.x);
            center.y = Mathf.Clamp(center.y, worldBoundsMin.y, worldBoundsMax.y);

            return center;
        }

        private float CalculateZoom(IReadOnlyList<GameObject> players)
        {
            if (players.Count <= 1) return minZoom;

            Bounds bounds = new Bounds(players[0].transform.position, Vector3.zero);
            foreach (var p in players)
            {
                if (p != null)
                    bounds.Encapsulate(p.transform.position);
            }

            float maxDimension = Mathf.Max(bounds.size.x, bounds.size.y);
            return Mathf.Clamp(maxDimension * zoomFactor, minZoom, maxZoom);
        }
    }
}
