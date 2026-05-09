using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerAnimation : MonoBehaviour
    {
        [Header("References")]
        public Transform visualRoot;
        public SpriteRenderer bodySprite;
        public SpriteRenderer hatSprite;

        [Header("Animation")]
        public float bobSpeed = 8f;
        public float bobHeight = 0.05f;
        public float rotationSpeed = 10f;

        [Header("Sprites")]
        public Sprite idleSprite;
        public Sprite walkSprite1;
        public Sprite walkSprite2;
        public Sprite jumpSprite;
        public Sprite fallSprite;
        public Sprite grabSprite;

        private PlayerController _controller;
        private Rigidbody2D _rb;
        private SpriteRenderer _mainRenderer;
        private float _walkTimer;
        private bool _facingRight = true;
        private bool _prevGrounded;

        private enum AnimState { Idle, Walk, Jump, Fall, Grab }
        private AnimState _currentState;

        private void Awake()
        {
            _controller = GetComponent<PlayerController>();
            _rb = GetComponent<Rigidbody2D>();
            _mainRenderer = bodySprite != null ? bodySprite : GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (_mainRenderer == null) return;

            UpdateFacing();
            UpdateAnimationState();
            ApplyVisualEffects();
        }

        private void UpdateFacing()
        {
            float h = InputManager.Instance?.GetHorizontal(_controller.PlayerIndex) ?? 0f;
            if (Mathf.Abs(h) > 0.1f)
            {
                _facingRight = h > 0f;
            }

            if (visualRoot != null)
            {
                Vector3 scale = visualRoot.localScale;
                scale.x = _facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
                visualRoot.localScale = scale;
            }
            else if (_mainRenderer != null)
            {
                _mainRenderer.flipX = !_facingRight;
                if (hatSprite != null) hatSprite.flipX = !_facingRight;
            }
        }

        private void UpdateAnimationState()
        {
            bool grounded = _controller.IsGrounded;
            float h = InputManager.Instance?.GetHorizontal(_controller.PlayerIndex) ?? 0f;
            float v = _rb.velocity.y;

            if (!grounded)
            {
                _currentState = v > 0f ? AnimState.Jump : AnimState.Fall;
            }
            else if (Mathf.Abs(h) > 0.1f)
            {
                _currentState = AnimState.Walk;
            }
            else
            {
                _currentState = AnimState.Idle;
            }

            // Override for grab
            var interactor = GetComponent<PlayerInteractor>();
            if (interactor != null && interactor.IsGrabbing())
            {
                _currentState = AnimState.Grab;
            }
        }

        private void ApplyVisualEffects()
        {
            switch (_currentState)
            {
                case AnimState.Idle:
                    IdleBob();
                    SetSprite(idleSprite);
                    break;

                case AnimState.Walk:
                    WalkAnimation();
                    break;

                case AnimState.Jump:
                    SetSprite(jumpSprite);
                    break;

                case AnimState.Fall:
                    SetSprite(fallSprite);
                    break;

                case AnimState.Grab:
                    SetSprite(grabSprite);
                    break;
            }
        }

        private void IdleBob()
        {
            if (visualRoot == null) return;
            Vector3 pos = visualRoot.localPosition;
            pos.y = Mathf.Sin(Time.time * bobSpeed * 0.5f) * bobHeight * 0.5f;
            visualRoot.localPosition = pos;
        }

        private void WalkAnimation()
        {
            _walkTimer += Time.deltaTime * bobSpeed;
            float cycle = Mathf.Sin(_walkTimer);

            if (visualRoot != null)
            {
                Vector3 pos = visualRoot.localPosition;
                pos.y = Mathf.Abs(cycle) * bobHeight;
                visualRoot.localPosition = pos;
            }

            SetSprite(cycle > 0f ? walkSprite1 : walkSprite2);
        }

        private void SetSprite(Sprite sprite)
        {
            if (_mainRenderer != null && sprite != null)
                _mainRenderer.sprite = sprite;
        }
    }
}
