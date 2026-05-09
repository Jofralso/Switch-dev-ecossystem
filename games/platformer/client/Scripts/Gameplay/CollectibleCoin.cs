using UnityEngine;

namespace Game.Gameplay
{
    public class CollectibleCoin : MonoBehaviour, ICollectible
    {
        [Header("Settings")]
        public int pointValue = 100;
        public float rotateSpeed = 180f;
        public float bobSpeed = 2f;
        public float bobHeight = 0.2f;

        [Header("Effects")]
        public GameObject collectParticle;
        public AudioClip collectSound;

        private bool _collected;
        private Vector3 _startPos;

        private void Start()
        {
            _startPos = transform.position;
        }

        private void Update()
        {
            if (_collected) return;

            transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
            transform.position = _startPos + Vector3.up * (Mathf.Sin(Time.time * bobSpeed) * bobHeight);
        }

        public void Collect(GameObject collector)
        {
            if (_collected) return;
            _collected = true;

            if (collectParticle != null)
                Instantiate(collectParticle, transform.position, Quaternion.identity);

            if (collectSound != null)
            {
                var audio = GetComponent<AudioSource>();
                if (audio != null) audio.PlayOneShot(collectSound);
            }

            Destroy(gameObject, 0.1f);
        }

        public bool IsCollected() => _collected;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_collected && other.CompareTag("Player"))
            {
                Collect(other.gameObject);
            }
        }
    }
}
