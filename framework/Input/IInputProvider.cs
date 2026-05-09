using UnityEngine;

namespace Framework.Input
{
    public interface IInputProvider
    {
        float GetHorizontal(int playerIndex);
        bool GetJumpDown(int playerIndex);
        bool GetJumpHeld(int playerIndex);
        bool GetInteractDown(int playerIndex);
        bool GetInteractHeld(int playerIndex);
        bool AnyKeyDown();
    }
}