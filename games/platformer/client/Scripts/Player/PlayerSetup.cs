using Game.Core;
using UnityEngine;

namespace Game.Player
{
    public class PlayerSetup : MonoBehaviour
    {
        [Header("Player Configuration")]
        public int playerIndex;
        public Color playerColor = Color.white;
        public bool configureOnStart = true;

        private void Start()
        {
            if (configureOnStart)
            {
                var controller = GetComponent<PlayerController>();
                if (controller != null)
                    controller.Initialize(playerIndex, transform.position);

                var renderer = GetComponentInChildren<SpriteRenderer>();
                if (renderer != null)
                    renderer.color = playerColor;
            }
        }
    }
}
