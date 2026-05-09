using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        private readonly Dictionary<int, PlayerInputBindings> _bindings = new();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            RegisterDefaults();
        }

        private void RegisterDefaults()
        {
            _bindings[0] = PlayerInputBindings.DefaultPlayer1();
            _bindings[1] = PlayerInputBindings.DefaultPlayer2();
            _bindings[2] = PlayerInputBindings.DefaultPlayer3();
            _bindings[3] = PlayerInputBindings.DefaultPlayer4();
        }

        public void BindPlayer(int playerIndex, PlayerInputBindings bindings)
        {
            _bindings[playerIndex] = bindings;
        }

        public float GetHorizontal(int playerIndex)
        {
            if (!_bindings.ContainsKey(playerIndex)) return 0f;
            var b = _bindings[playerIndex];
            float val = 0f;
            if (Input.GetKey(b.moveLeft)) val -= 1f;
            if (Input.GetKey(b.moveRight)) val += 1f;
            val += Input.GetAxisRaw("Horizontal_" + playerIndex);
            return Mathf.Clamp(val, -1f, 1f);
        }

        public bool GetJumpDown(int playerIndex)
        {
            if (!_bindings.ContainsKey(playerIndex)) return false;
            var b = _bindings[playerIndex];
            return Input.GetKeyDown(b.jump) || Input.GetButtonDown("Jump_" + playerIndex);
        }

        public bool GetJumpHeld(int playerIndex)
        {
            if (!_bindings.ContainsKey(playerIndex)) return false;
            var b = _bindings[playerIndex];
            return Input.GetKey(b.jump) || Input.GetButton("Jump_" + playerIndex);
        }

        public bool GetInteractDown(int playerIndex)
        {
            if (!_bindings.ContainsKey(playerIndex)) return false;
            var b = _bindings[playerIndex];
            return Input.GetKeyDown(b.interact) || Input.GetButtonDown("Interact_" + playerIndex);
        }

        public bool GetInteractHeld(int playerIndex)
        {
            if (!_bindings.ContainsKey(playerIndex)) return false;
            var b = _bindings[playerIndex];
            return Input.GetKey(b.interact) || Input.GetButton("Interact_" + playerIndex);
        }

        public bool AnyKeyDown()
        {
            return Input.anyKeyDown;
        }
    }
}
