using Game.Core;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class LevelDataCreator : EditorWindow
    {
        private string _levelName = "New Level";
        private int _levelNumber = 1;
        private int _requiredKeys = 1;
        private int _maxPlayers = 4;
        private float _parTime = 60f;
        private bool _isTutorial;

        [MenuItem("Game/Create Level Data")]
        public static void ShowWindow()
        {
            GetWindow<LevelDataCreator>("Level Data Creator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Create Level Data Asset", EditorStyles.boldLabel);

            _levelName = EditorGUILayout.TextField("Level Name", _levelName);
            _levelNumber = EditorGUILayout.IntField("Level Number", _levelNumber);
            _requiredKeys = EditorGUILayout.IntField("Required Keys", _requiredKeys);
            _maxPlayers = EditorGUILayout.IntSlider("Max Players", _maxPlayers, 1, 4);
            _parTime = EditorGUILayout.FloatField("Par Time", _parTime);
            _isTutorial = EditorGUILayout.Toggle("Is Tutorial", _isTutorial);

            GUILayout.Space(10);

            if (GUILayout.Button("Create Level Data", GUILayout.Height(30)))
            {
                CreateAsset();
            }
        }

        private void CreateAsset()
        {
            var data = ScriptableObject.CreateInstance<LevelData>();
            data.levelName = _levelName;
            data.levelNumber = _levelNumber;
            data.requiredKeys = _requiredKeys;
            data.playerCount = _maxPlayers;
            data.parTime = _parTime;
            data.isTutorial = _isTutorial;

            string path = $"Assets/Resources/Levels/Level_{_levelNumber:D2}.asset";
            System.IO.Directory.CreateDirectory("Assets/Resources/Levels");

            AssetDatabase.CreateAsset(data, path);
            AssetDatabase.SaveAssets();

            Selection.activeObject = data;
            EditorGUIUtility.PingObject(data);

            Debug.Log($"Level data created: {path}");
        }
    }
}
