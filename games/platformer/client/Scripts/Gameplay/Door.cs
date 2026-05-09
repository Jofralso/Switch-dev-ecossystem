using Game.Core;
using UnityEngine;

namespace Game.Gameplay
{
    public class Door : MonoBehaviour
    {
        [Header("Door Settings")]
        public float openSpeed = 2f;
        public Vector3 openOffset = Vector3.up * 3f;
        public bool requireAllPlayers = true;
        public ParticleSystem openEffect;

        [Header("Visual")]
        public SpriteRenderer doorRenderer;
        public Sprite closedSprite;
        public Sprite openSprite;
        public Color lockedColor = Color.red;
        public Color unlockedColor = Color.green;

        [Header("Audio")]
        public AudioClip openSound;
        public AudioSource audioSource;

        private bool _isOpen;
        private bool _isLocked = true;
        private int _keysRequired;
        private int _keysUnlocked;
        private Vector3 _closedPosition;
        private Vector3 _openPosition;

        private void Start()
        {
            _closedPosition = transform.position;
            _openPosition = _closedPosition + openOffset;

            if (doorRenderer != null)
                doorRenderer.sprite = closedSprite;
        }

        public void Lock(int keysRequired)
        {
            _keysRequired = keysRequired;
            _isLocked = true;
            _keysUnlocked = 0;

            if (doorRenderer != null)
                doorRenderer.color = lockedColor;
        }

        public void Unlock(int totalKeys)
        {
            _keysUnlocked = totalKeys;
            if (_keysUnlocked >= _keysRequired)
            {
                _isLocked = false;
                if (doorRenderer != null)
                {
                    doorRenderer.color = unlockedColor;
                    doorRenderer.sprite = openSprite;
                }
            }
        }

        public bool IsLocked() => _isLocked;
        public bool IsOpen() => _isOpen;

        private void Update()
        {
            if (!_isLocked && !_isOpen)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position, _openPosition, openSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, _openPosition) < 0.01f)
                {
                    _isOpen = true;

                    if (openEffect != null)
                        openEffect.Play();

                    if (openSound != null && audioSource != null)
                        audioSource.PlayOneShot(openSound);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isOpen || !other.CompareTag("Player")) return;

            var controller = other.GetComponent<Player.PlayerController>();
            if (controller != null)
            {
                controller.MarkAtExit(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            var controller = other.GetComponent<Player.PlayerController>();
            if (controller != null)
            {
                controller.MarkAtExit(false);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Vector3 openPos = Application.isPlaying
                ? _openPosition
                : transform.position + openOffset;
            Gizmos.DrawWireSphere(openPos, 0.5f);
            Gizmos.DrawLine(transform.position, openPos);
        }
    }
}
