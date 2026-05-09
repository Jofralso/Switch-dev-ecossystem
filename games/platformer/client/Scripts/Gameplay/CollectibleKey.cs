using Game.Core;
using UnityEngine;

namespace Game.Gameplay
{
    public class CollectibleKey : MonoBehaviour, ICollectible, IGrabbable
    {
        [Header("Settings")]
        public float collectRadius = 0.5f;
        public float bobSpeed = 2f;
        public float bobHeight = 0.15f;
        public bool destroyOnCollect = true;

        [Header("Effects")]
        public GameObject collectEffect;
        public AudioClip collectSound;

        private bool _collected;
        private Vector3 _startPos;
        private Collider2D _collider;

        private void Start()
        {
            _startPos = transform.position;
            _collider = GetComponent<Collider2D>();
        }

        private void Update()
        {
            if (!_collected)
            {
                transform.position = _startPos + Vector3.up * (Mathf.Sin(Time.time * bobSpeed) * bobHeight);
                transform.Rotate(Vector3.up, 90f * Time.deltaTime);
            }
        }

        public void Collect(GameObject collector)
        {
            if (_collected) return;
            _collected = true;

            var levelManager = FindObjectOfType<LevelManager>();
            levelManager?.CollectKey();

            if (collectEffect != null)
                Instantiate(collectEffect, transform.position, Quaternion.identity);

            if (destroyOnCollect)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);
        }

        public bool IsCollected() => _collected;

        public bool CanGrab() => !_collected;

        public void OnGrabbed(GameObject grabber)
        {
            if (_collider != null)
                _collider.enabled = false;
        }

        public void OnReleased()
        {
            if (_collider != null)
                _collider.enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_collected && other.CompareTag("Player"))
            {
                Collect(other.gameObject);
            }
        }
    }
}
