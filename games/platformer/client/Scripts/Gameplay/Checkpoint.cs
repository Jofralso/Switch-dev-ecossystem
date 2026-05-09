using UnityEngine;

namespace Game.Gameplay
{
    public class Checkpoint : MonoBehaviour
    {
        [Header("Checkpoint")]
        public int checkpointId;
        public SpriteRenderer flagRenderer;
        public Sprite inactiveSprite;
        public Sprite activeSprite;
        public Color activeColor = Color.green;
        public Color inactiveColor = Color.gray;

        private bool _activated;

        private void Start()
        {
            if (flagRenderer != null)
            {
                flagRenderer.sprite = inactiveSprite;
                flagRenderer.color = inactiveColor;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_activated || !other.CompareTag("Player")) return;

            _activated = true;

            if (flagRenderer != null)
            {
                flagRenderer.sprite = activeSprite;
                flagRenderer.color = activeColor;
            }

            var levelManager = FindObjectOfType<Core.LevelManager>();
            if (levelManager != null && checkpointId > 0)
            {
                PlayerPrefs.SetInt($"Checkpoint_{Core.GameManager.Instance?.GetCurrentLevel()}", checkpointId);
                PlayerPrefs.Save();
            }
        }

        public bool IsActivated() => _activated;
    }
}
