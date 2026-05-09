using UnityEngine;

namespace Game.Core
{
    [ExecuteAlways]
    public class ProjectSetup : MonoBehaviour
    {
        [Header("Project Configuration")]
        public bool configureTagsAndLayers = true;

        private void Awake()
        {
            if (configureTagsAndLayers)
                ConfigureTags();
        }

        private void ConfigureTags()
        {
            AddTag("Player");
            AddTag("Enemy");
            AddTag("Key");
            AddTag("Door");
            AddTag("Ground");
            AddTag("Wall");
            AddTag("Hazard");
            AddTag("Platform");
            AddTag("Checkpoint");
            AddTag("FinishLine");
        }

        private static void AddTag(string tag)
        {
#if UNITY_EDITOR
            var tags = UnityEditor.TagManager.GetDefinedTags();
            if (!System.Array.Exists(tags, t => t == tag))
            {
                var newTags = new string[tags.Length + 1];
                System.Array.Copy(tags, newTags, tags.Length);
                newTags[^1] = tag;
                UnityEditor.TagManager.SetDefinedTags(newTags);
                Debug.Log($"Added tag: {tag}");
            }
#endif
        }
    }
}
