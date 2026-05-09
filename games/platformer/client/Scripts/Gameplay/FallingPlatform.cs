using UnityEngine;

namespace Game.Gameplay
{
    public class FallingPlatform : MonoBehaviour
    {
        [Header("Fall Settings")]
        public float fallDelay = 0.5f;
        public float respawnDelay = 3f;
        public float shakeIntensity = 0.05f;

        private Rigidbody2D _rb;
        private Vector3 _startPosition;
        private bool _isFalling;
        private bool _isShaking;
        private float _timer;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _startPosition = transform.position;
            if (_rb != null)
            {
                _rb.bodyType = RigidbodyType2D.Static;
                _rb.isKinematic = true;
            }
        }

        private void Update()
        {
            if (_isShaking)
            {
                _timer -= Time.deltaTime;
                transform.position = _startPosition + (Vector3)Random.insideUnitCircle * shakeIntensity;

                if (_timer <= 0f)
                {
                    _isShaking = false;
                    _isFalling = true;
                    if (_rb != null)
                    {
                        _rb.isKinematic = false;
                        _rb.bodyType = RigidbodyType2D.Dynamic;
                    }
                }
            }

            if (_isFalling && _rb != null)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                {
                    ResetPlatform();
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_isShaking || _isFalling) return;

            if (collision.collider.CompareTag("Player"))
            {
                _isShaking = true;
                _timer = fallDelay;
            }
        }

        private void ResetPlatform()
        {
            _isFalling = false;
            _isShaking = false;
            transform.position = _startPosition;
            if (_rb != null)
            {
                _rb.isKinematic = true;
                _rb.bodyType = RigidbodyType2D.Static;
                _rb.velocity = Vector2.zero;
            }
            _timer = respawnDelay;
        }
    }
}
