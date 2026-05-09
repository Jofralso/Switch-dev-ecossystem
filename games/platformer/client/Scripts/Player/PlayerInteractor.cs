using Game.Core;
using Game.Gameplay;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerInteractor : MonoBehaviour
    {
        [Header("Interaction")]
        public float grabRange = 0.8f;
        public float grabRadius = 0.5f;
        public LayerMask interactableLayer = ~0;

        private PlayerController _controller;
        private Rigidbody2D _rb;
        private IInteractable _currentInteractable;
        private GameObject _grabbedObject;
        private FixedJoint2D _grabJoint;

        private void Awake()
        {
            _controller = GetComponent<PlayerController>();
            _rb = GetComponent<Rigidbody2D>();
        }

        public void TryInteract()
        {
            var hit = Physics2D.OverlapCircle(transform.position, grabRange, interactableLayer);
            if (hit == null) return;

            var interactable = hit.GetComponent<IInteractable>();
            interactable?.Interact(gameObject);
        }

        public void TryGrab()
        {
            if (_grabbedObject != null) return;

            var hit = Physics2D.OverlapCircle(transform.position, grabRange, interactableLayer);
            if (hit == null) return;

            var grabbable = hit.GetComponent<IGrabbable>();
            if (grabbable == null || !grabbable.CanGrab()) return;

            _grabbedObject = hit.gameObject;
            _grabJoint = _grabbedObject.AddComponent<FixedJoint2D>();
            _grabJoint.connectedBody = _rb;
            _grabJoint.autoConfigureConnectedAnchor = false;
            _grabJoint.anchor = Vector2.zero;
            _grabJoint.connectedAnchor = Vector2.zero;
            _grabJoint.breakForce = 500f;
            _grabJoint.breakTorque = 500f;

            grabbable.OnGrabbed(gameObject);
        }

        public void ReleaseGrab()
        {
            if (_grabbedObject == null) return;

            var grabbable = _grabbedObject.GetComponent<IGrabbable>();
            grabbable?.OnReleased();

            if (_grabJoint != null)
            {
                Destroy(_grabJoint);
            }

            _grabbedObject = null;
            _grabJoint = null;
        }

        public GameObject GetGrabbedObject() => _grabbedObject;
        public bool IsGrabbing() => _grabbedObject != null;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, grabRange);
        }
    }
}
