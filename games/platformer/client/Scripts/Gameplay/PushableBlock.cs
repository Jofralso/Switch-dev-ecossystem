using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PushableBlock : MonoBehaviour, IGrabbable
    {
        [Header("Physics")]
        public float maxPushSpeed = 3f;
        public float pushDrag = 5f;
        public bool canBeGrabbed = true;

        private Rigidbody2D _rb;
        private Vector2 _startPosition;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _startPosition = transform.position;
        }

        private void FixedUpdate()
        {
            if (_rb.velocity.magnitude > maxPushSpeed)
                _rb.velocity = _rb.velocity.normalized * maxPushSpeed;
        }

        public bool CanGrab() => canBeGrabbed;

        public void OnGrabbed(GameObject grabber)
        {
            _rb.drag = 0f;
        }

        public void OnReleased()
        {
            _rb.drag = pushDrag;
        }

        public void ResetPosition()
        {
            transform.position = _startPosition;
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0f;
        }

        public Rigidbody2D GetRigidbody() => _rb;
    }
}
