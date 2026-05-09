using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class MovingPlatform : MonoBehaviour
    {
        [Header("Movement")]
        public Transform[] waypoints;
        public float moveSpeed = 2f;
        public float waitTime = 1f;
        public bool startActive = true;
        public bool loop = true;

        [Header("Passengers")]
        public LayerMask passengerLayers = ~0;

        private int _currentWaypoint;
        private int _direction = 1;
        private float _waitTimer;
        private bool _active;
        private readonly HashSet<Transform> _passengers = new();

        private void Start()
        {
            _active = startActive;
            if (waypoints.Length > 0)
                transform.position = waypoints[0].position;
        }

        private void Update()
        {
            if (!_active || waypoints.Length == 0) return;

            if (_waitTimer > 0f)
            {
                _waitTimer -= Time.deltaTime;
                return;
            }

            Transform target = waypoints[_currentWaypoint];
            Vector3 targetPos = target.position;
            transform.position = Vector3.MoveTowards(
                transform.position, targetPos, moveSpeed * Time.deltaTime);

            // Move passengers along with platform
            foreach (var p in _passengers)
            {
                if (p != null)
                {
                    p.position += (Vector3)(GetComponent<Rigidbody2D>()?.velocity ?? Vector2.zero) * Time.deltaTime;
                }
            }

            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                _waitTimer = waitTime;
                AdvanceWaypoint();
            }
        }

        private void AdvanceWaypoint()
        {
            if (loop)
            {
                _currentWaypoint = (_currentWaypoint + _direction) % waypoints.Length;
                if (_currentWaypoint < 0)
                    _currentWaypoint = waypoints.Length - 1;
            }
            else
            {
                _currentWaypoint += _direction;
                if (_currentWaypoint >= waypoints.Length || _currentWaypoint < 0)
                {
                    _direction *= -1;
                    _currentWaypoint += _direction;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (LayerInMask(collision.gameObject.layer, passengerLayers))
            {
                _passengers.Add(collision.transform);
                collision.transform.SetParent(transform, true);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            _passengers.Remove(collision.transform);
            collision.transform.SetParent(null);
        }

        public void Activate() => _active = true;
        public void Deactivate() => _active = false;
        public bool IsActive() => _active;

        private static bool LayerInMask(int layer, LayerMask mask) =>
            (mask & (1 << layer)) != 0;

        private void OnDrawGizmosSelected()
        {
            if (waypoints == null) return;
            Gizmos.color = Color.cyan;
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] == null) continue;
                Gizmos.DrawWireSphere(waypoints[i].position, 0.3f);
                if (i < waypoints.Length - 1 && waypoints[i + 1] != null)
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
    }
}
