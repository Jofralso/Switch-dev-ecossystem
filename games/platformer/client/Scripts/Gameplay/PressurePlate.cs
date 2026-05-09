using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class PressurePlate : MonoBehaviour
    {
        [Header("Settings")]
        public float requiredWeight = 1f;
        public bool requireAllPlayers;
        public float pressDepth = 0.1f;
        public LayerMask triggerLayers = ~0;

        [Header("Targets")]
        public MonoBehaviour[] targets;
        public string activateMethod = "Activate";
        public string deactivateMethod = "Deactivate";

        [Header("Visual")]
        public SpriteRenderer plateRenderer;
        public Sprite pressedSprite;
        public Sprite unpressedSprite;
        public Color pressedColor = Color.green;
        public Color unpressedColor = Color.red;

        private int _objectsOnPlate;
        private bool _isPressed;
        private Vector3 _startPos;
        private Vector3 _pressedPos;

        private void Start()
        {
            _startPos = transform.position;
            _pressedPos = _startPos - Vector3.up * pressDepth;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerInMask(other.gameObject.layer, triggerLayers)) return;
            _objectsOnPlate++;
            UpdateState();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!LayerInMask(other.gameObject.layer, triggerLayers)) return;
            _objectsOnPlate--;
            UpdateState();
        }

        private void UpdateState()
        {
            bool shouldBePressed;

            if (requireAllPlayers)
            {
                int playerCount = Game.Core.GameManager.Instance?.activePlayerCount ?? 1;
                shouldBePressed = _objectsOnPlate >= playerCount;
            }
            else
            {
                shouldBePressed = _objectsOnPlate >= requiredWeight;
            }

            if (shouldBePressed != _isPressed)
            {
                _isPressed = shouldBePressed;
                NotifyTargets(_isPressed);
                UpdateVisuals();
            }
        }

        private void NotifyTargets(bool pressed)
        {
            foreach (var t in targets)
            {
                if (t == null) continue;
                string method = pressed ? activateMethod : deactivateMethod;
                t.SendMessage(method, SendMessageOptions.DontRequireReceiver);
            }
        }

        private void UpdateVisuals()
        {
            transform.position = _isPressed ? _pressedPos : _startPos;

            if (plateRenderer != null)
            {
                plateRenderer.sprite = _isPressed ? pressedSprite : unpressedSprite;
                plateRenderer.color = _isPressed ? pressedColor : unpressedColor;
            }
        }

        private static bool LayerInMask(int layer, LayerMask mask) =>
            (mask & (1 << layer)) != 0;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _isPressed ? Color.green : Color.red;
            Gizmos.DrawWireCube(transform.position - Vector3.up * pressDepth, GetComponent<Collider2D>()?.bounds.size ?? Vector3.one);
        }
    }
}
