using Game.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class LevelSelectUI : MonoBehaviour
    {
        [Header("References")]
        public GameObject levelButtonPrefab;
        public Transform buttonContainer;
        public Sprite lockedSprite;
        public Sprite completedSprite;
        public Sprite unlockedSprite;

        private void Start()
        {
            PopulateLevelButtons();
        }

        private void PopulateLevelButtons()
        {
            var gm = GameManager.Instance;
            if (gm == null) return;

            int highestUnlocked = gm.GetHighestUnlockedLevel();
            var results = gm.GetLevelResults();

            foreach (Transform child in buttonContainer)
                Destroy(child.gameObject);

            foreach (var level in gm.levels)
            {
                var btnObj = Instantiate(levelButtonPrefab, buttonContainer);
                var btn = btnObj.GetComponent<Button>();
                var img = btnObj.GetComponent<Image>();
                var text = btnObj.GetComponentInChildren<Text>();

                int levelNum = level.levelNumber;
                bool isUnlocked = levelNum <= highestUnlocked;
                bool isCompleted = results.Exists(r => r.levelNumber == levelNum && r.completed);

                if (img != null)
                {
                    if (isCompleted) img.sprite = completedSprite;
                    else if (isUnlocked) img.sprite = unlockedSprite;
                    else img.sprite = lockedSprite;
                }

                if (text != null)
                    text.text = $"{levelNum}";

                btn.interactable = isUnlocked;
                btn.onClick.AddListener(() => gm.LoadLevel(levelNum - 1));
            }
        }
    }
}
