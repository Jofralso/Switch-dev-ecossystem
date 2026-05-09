using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(fileName = "LevelDatabase", menuName = "Game/LevelDatabase")]
    public class LevelDatabase : ScriptableObject
    {
        public List<LevelData> levels = new();

        public LevelData GetLevel(int index)
        {
            if (index >= 0 && index < levels.Count)
                return levels[index];
            return null;
        }

        public int GetLevelIndex(LevelData data)
        {
            return levels.IndexOf(data);
        }

        public int TotalLevels => levels.Count;
    }
}
