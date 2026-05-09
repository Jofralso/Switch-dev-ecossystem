using Game.Core;
using Game.Player;
using UnityEngine;

namespace Game.Gameplay
{
    public class Hazard : MonoBehaviour
    {
        [Header("Hazard Settings")]
        public bool instantKill = true;
        public float damageAmount = 1f;
        public bool respawnOnTouch = true;
        public float knockbackForce = 10f;

        [Header("Visual")]
        public SpriteRenderer hazardRenderer;
        public Color activeColor = Color.red;
        public Color inactiveColor = Color.gray;

        [Header("Animation")]
        public float pulseSpeed = 2f;
        public float pulseAmount = 0.1f;

        private bool _active = true;
        private Vector3 _startScale;

        private void Start()
        {
            _startScale = transform.localScale;
        }

        private void Update()
        {
            if (!_active) return;

            float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            transform.localScale = _startScale * pulse;

            if (hazardRenderer != null)
                hazardRenderer.color = Color.Lerp(activeColor, Color.white, Mathf.PingPong(Time.time * pulseSpeed, 1f));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_active) return;
            HandleCollision(other);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_active) return;
            HandleCollision(collision.collider);
        }

        private void HandleCollision(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            var controller = other.GetComponent<PlayerController>();
            if (controller == null) return;

            if (instantKill || respawnOnTouch)
            {
                controller.Die();
            }
            else
            {
                var rb = controller.GetRigidbody();
                if (rb != null)
                {
                    Vector2 knockDir = (rb.position - (Vector2)transform.position).normalized;
                    rb.velocity = knockDir * knockbackForce;
                }

                var damageable = other.GetComponent<IDamageable>();
                damageable?.TakeDamage(gameObject);
            }
        }

        public void Activate()
        {
            _active = true;
            if (hazardRenderer != null)
                hazardRenderer.color = activeColor;
        }

        public void Deactivate()
        {
            _active = false;
            if (hazardRenderer != null)
                hazardRenderer.color = inactiveColor;
        }

        public bool IsActive() => _active;
    }
}
