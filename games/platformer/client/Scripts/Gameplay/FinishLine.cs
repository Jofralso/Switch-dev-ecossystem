using Game.Core;
using Game.Player;
using UnityEngine;

namespace Game.Gameplay
{
    public class FinishLine : MonoBehaviour
    {
        [Header("Settings")]
        public bool requireAllPlayers = true;
        public float holdDuration = 1f;
        public ParticleSystem finishEffect;

        private int _playersInZone;
        private float _holdTimer;

        private void Update()
        {
            if (requireAllPlayers && _playersInZone > 0)
            {
                var gm = GameManager.Instance;
                if (gm == null) return;

                int total = gm.activePlayerCount;
                if (_playersInZone >= total)
                {
                    _holdTimer += Time.deltaTime;
                    if (_holdTimer >= holdDuration)
                    {
                        CompleteLevel();
                    }
                }
                else
                {
                    _holdTimer = 0f;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            _playersInZone++;
            var controller = other.GetComponent<PlayerController>();
            if (controller != null)
                controller.MarkAtExit(true);

            if (!requireAllPlayers && _playersInZone >= 1)
            {
                CompleteLevel();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            _playersInZone--;
            var controller = other.GetComponent<PlayerController>();
            if (controller != null)
                controller.MarkAtExit(false);
        }

        private void CompleteLevel()
        {
            var gm = GameManager.Instance;
            if (gm == null) return;

            if (finishEffect != null)
                finishEffect.Play();

            gm.state = GameState.LevelComplete;
        }
    }
}
