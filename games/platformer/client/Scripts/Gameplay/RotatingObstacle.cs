using UnityEngine;

namespace Game.Gameplay
{
    public class RotatingObstacle : MonoBehaviour
    {
        [Header("Rotation")]
        public float rotationSpeed = 90f;
        public bool clockwise = true;
        public bool active = true;

        [Header("Damage")]
        public bool killsOnContact = true;
        public float knockbackForce = 15f;

        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!active) return;

            float direction = clockwise ? -1f : 1f;
            float torque = rotationSpeed * direction * Time.fixedDeltaTime;

            if (_rb != null)
            {
                _rb.angularVelocity = rotationSpeed * direction;
            }
            else
            {
                transform.Rotate(0f, 0f, rotationSpeed * direction * Time.deltaTime);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!active || !killsOnContact) return;

            if (collision.collider.CompareTag("Player"))
            {
                var controller = collision.collider.GetComponent<Player.PlayerController>();
                if (controller != null)
                {
                    controller.Die();
                }
            }
        }

        public void Activate() => active = true;
        public void Deactivate() => active = false;
    }
}
