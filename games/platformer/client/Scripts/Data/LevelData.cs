using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Game/LevelData")]
    public class LevelData : ScriptableObject
    {
        public string levelName;
        public int levelNumber;
        public SceneReference scene;
        public Sprite thumbnail;
        public float parTime = 60f;
        public int requiredKeys = 1;
        public int playerCount = 4;
        public bool isTutorial;
        [TextArea] public string description;
    }

    [System.Serializable]
    public struct SceneReference
    {
        public string scenePath;
        public string sceneGUID;
    }

    [System.Serializable]
    public struct LevelResult
    {
        public int levelNumber;
        public float completionTime;
        public int deaths;
        public bool completed;
    }

    [System.Serializable]
    public class LevelProgress
    {
        public List<LevelResult> results = new();
        public int highestLevelUnlocked;

        public void MarkCompleted(int level, float time, int deaths)
        {
            var existing = results.Find(r => r.levelNumber == level);
            if (existing.completed)
            {
                if (time < existing.completionTime)
                {
                    results.Remove(existing);
                    results.Add(new LevelResult
                    {
                        levelNumber = level,
                        completionTime = time,
                        deaths = deaths,
                        completed = true
                    });
                }
            }
            else
            {
                results.Add(new LevelResult
                {
                    levelNumber = level,
                    completionTime = time,
                    deaths = deaths,
                    completed = true
                });
            }

            if (level >= highestLevelUnlocked)
                highestLevelUnlocked = level + 1;
        }
    }
}
