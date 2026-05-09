using UnityEngine;

namespace Game.Core
{
    [System.Serializable]
    public struct PlayerInputBindings
    {
        public KeyCode moveLeft;
        public KeyCode moveRight;
        public KeyCode jump;
        public KeyCode interact;

        public static PlayerInputBindings DefaultPlayer1() => new()
        {
            moveLeft = KeyCode.A,
            moveRight = KeyCode.D,
            jump = KeyCode.W,
            interact = KeyCode.Space
        };

        public static PlayerInputBindings DefaultPlayer2() => new()
        {
            moveLeft = KeyCode.LeftArrow,
            moveRight = KeyCode.RightArrow,
            jump = KeyCode.UpArrow,
            interact = KeyCode.Return
        };

        public static PlayerInputBindings DefaultPlayer3() => new()
        {
            moveLeft = KeyCode.J,
            moveRight = KeyCode.L,
            jump = KeyCode.I,
            interact = KeyCode.U
        };

        public static PlayerInputBindings DefaultPlayer4() => new()
        {
            moveLeft = KeyCode.Keypad4,
            moveRight = KeyCode.Keypad6,
            jump = KeyCode.Keypad8,
            interact = KeyCode.Keypad5
        };
    }

    public enum PlayerInputDevice
    {
        Keyboard,
        Gamepad
    }
}
