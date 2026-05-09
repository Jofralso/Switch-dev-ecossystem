using Game.Core;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerInteractor))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 8f;
        public float jumpForce = 14f;
        public float groundCheckDistance = 0.15f;
        public LayerMask groundLayer = ~0;
        public float coyoteTime = 0.1f;
        public float jumpBufferTime = 0.1f;

        [Header("Physics")]
        public float pushForce = 5f;
        public float friction = 0.5f;
        public int maxPlayerPush = 2;

        [Header("Visual")]
        public SpriteRenderer bodyRenderer;
        public Color[] playerColors;

        public int PlayerIndex { get; private set; }
        public bool IsGrounded { get; private set; }

        private Rigidbody2D _rb;
        private PlayerInteractor _interactor;
        private InputManager _input;
        private Vector2 _spawnPosition;

        private float _coyoteTimer;
        private float _jumpBufferTimer;
        private bool _wantsJump;
        private bool _isRespawning;
        private bool _atExitDoor;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _interactor = GetComponent<PlayerInteractor>();
            _input = InputManager.Instance;
        }

        public void Initialize(int playerIndex, Vector2 spawnPos)
        {
            PlayerIndex = playerIndex;
            _spawnPosition = spawnPos;
            _atExitDoor = false;

            if (bodyRenderer != null && playerIndex < playerColors.Length)
                bodyRenderer.color = playerColors[playerIndex];

            gameObject.name = $"Player_{playerIndex + 1}";
        }

        private void Update()
        {
            if (GameManager.Instance.state != GameState.Playing) return;
            if (_isRespawning) return;

            CheckGrounded();

            _coyoteTimer = IsGrounded ? coyoteTime : _coyoteTimer - Time.deltaTime;

            if (_input.GetJumpDown(PlayerIndex))
                _jumpBufferTimer = jumpBufferTime;
            else
                _jumpBufferTimer -= Time.deltaTime;

            if (_jumpBufferTimer > 0f && _coyoteTimer > 0f)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
                _coyoteTimer = 0f;
                _jumpBufferTimer = 0f;
            }

            // Variable jump height
            if (_input.GetJumpHeld(PlayerIndex) && _rb.velocity.y > 0f)
            {
                _rb.gravityScale = 1f;
            }
            else
            {
                _rb.gravityScale = 2f;
            }

            if (_input.GetInteractDown(PlayerIndex))
                _interactor.TryInteract();

            if (_input.GetInteractHeld(PlayerIndex))
                _interactor.TryGrab();
            else
                _interactor.ReleaseGrab();
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.state != GameState.Playing) return;
            if (_isRespawning) return;

            float horizontal = _input.GetHorizontal(PlayerIndex);
            Vector2 targetVelocity = new Vector2(horizontal * moveSpeed, _rb.velocity.y);
            _rb.velocity = targetVelocity;

            // Apply friction when no input
            if (Mathf.Abs(horizontal) < 0.01f && IsGrounded)
            {
                _rb.velocity = new Vector2(_rb.velocity.x * (1f - friction * Time.fixedDeltaTime), _rb.velocity.y);
            }

            // Handle player-to-player pushing
            HandlePlayerCollision();
        }

        private void CheckGrounded()
        {
            var hit = Physics2D.Raycast(
                transform.position, Vector2.down, groundCheckDistance, groundLayer);
            IsGrounded = hit.collider != null;
        }

        private void HandlePlayerCollision()
        {
            var hit = Physics2D.OverlapCircleAll(transform.position, 0.6f);
            foreach (var col in hit)
            {
                if (col.gameObject == gameObject) continue;
                if (!col.CompareTag("Player")) continue;

                var otherRb = col.GetComponent<Rigidbody2D>();
                if (otherRb == null) continue;

                Vector2 pushDir = (col.transform.position - transform.position).normalized;
                float pushMagnitude = pushForce * Time.fixedDeltaTime;
                otherRb.velocity += pushDir * pushMagnitude;
            }
        }

        public void Die()
        {
            if (_isRespawning) return;
            _isRespawning = true;

            GameManager.Instance.RegisterDeath();

            var levelManager = FindObjectOfType<LevelManager>();
            if (levelManager != null)
            {
                Invoke(nameof(DoRespawn), levelManager.respawnDelay);
            }
            else
            {
                DoRespawn();
            }
        }

        private void DoRespawn()
        {
            var levelManager = FindObjectOfType<LevelManager>();
            if (levelManager != null)
            {
                levelManager.RespawnPlayer(this);
            }
            else
            {
                Respawn(_spawnPosition);
            }
            _isRespawning = false;
        }

        public void Respawn(Vector2 position)
        {
            transform.position = position;
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0f;
            _atExitDoor = false;
        }

        public void MarkAtExit(bool atExit)
        {
            _atExitDoor = atExit;
        }

        public bool IsAtExit() => _atExitDoor;

        public Rigidbody2D GetRigidbody() => _rb;
    }
}
