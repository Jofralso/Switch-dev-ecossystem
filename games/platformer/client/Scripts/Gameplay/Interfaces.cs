using UnityEngine;

namespace Game.Gameplay
{
    public interface IInteractable
    {
        void Interact(GameObject interactor);
    }

    public interface IGrabbable
    {
        bool CanGrab();
        void OnGrabbed(GameObject grabber);
        void OnReleased();
    }

    public interface IDamageable
    {
        void TakeDamage(GameObject source);
    }

    public interface ICollectible
    {
        void Collect(GameObject collector);
        bool IsCollected();
    }

    public interface IActivatable
    {
        void Activate();
        void Deactivate();
        bool IsActive();
    }
}
