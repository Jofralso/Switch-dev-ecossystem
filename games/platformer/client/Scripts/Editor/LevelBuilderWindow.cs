using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class LevelBuilderWindow : EditorWindow
    {
        private int _playerCount = 4;
        private int _keyCount = 1;
        private float _levelWidth = 20f;
        private float _levelHeight = 10f;
        private GameObject _levelRoot;
        private Color _backgroundColor = new(0.1f, 0.1f, 0.15f);

        [MenuItem("Game/Level Builder")]
        public static void ShowWindow()
        {
            GetWindow<LevelBuilderWindow>("Level Builder");
        }

        private void OnGUI()
        {
            GUILayout.Label("Level Settings", EditorStyles.boldLabel);

            _playerCount = EditorGUILayout.IntSlider("Player Count", _playerCount, 1, 4);
            _keyCount = EditorGUILayout.IntField("Required Keys", _keyCount);
            _levelWidth = EditorGUILayout.FloatField("Level Width", _levelWidth);
            _levelHeight = EditorGUILayout.FloatField("Level Height", _levelHeight);
            _backgroundColor = EditorGUILayout.ColorField("Background Color", _backgroundColor);
            _levelRoot = (GameObject)EditorGUILayout.ObjectField("Level Root", _levelRoot, typeof(GameObject), true);

            GUILayout.Space(10);

            if (GUILayout.Button("Create Level Scene", GUILayout.Height(30)))
            {
                CreateLevelScene();
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Add Player Spawn Points", GUILayout.Height(25)))
            {
                AddSpawnPoints();
            }

            if (GUILayout.Button("Add Exit Door", GUILayout.Height(25)))
            {
                AddExitDoor();
            }

            if (GUILayout.Button("Add Key Placeholders", GUILayout.Height(25)))
            {
                AddKeyPlaceholders();
            }
        }

        private void CreateLevelScene()
        {
            var root = new GameObject("Level_Root");
            var bg = new GameObject("Background");
            bg.transform.SetParent(root.transform);

            var bgRenderer = bg.AddComponent<SpriteRenderer>();
            bgRenderer.color = _backgroundColor;

            var bgCollider = bg.AddComponent<BoxCollider2D>();
            bgCollider.size = new Vector2(_levelWidth, _levelHeight);
            bgCollider.isTrigger = true;

            var walls = CreateWalls(root.transform);
            var ground = CreateGround(root.transform);

            var levelManager = root.AddComponent<Core.LevelManager>();
            levelManager.requiredKeys = _keyCount;

            AddSpawnPointsTo(root.transform);

            Selection.activeGameObject = root;
        }

        private GameObject CreateWalls(Transform parent)
        {
            var walls = new GameObject("Walls");
            walls.transform.SetParent(parent);

            float t = 0.5f;
            CreateWallSegment(walls.transform, new Vector2(0, _levelHeight / 2f), new Vector2(_levelWidth + 1f, t));
            CreateWallSegment(walls.transform, new Vector2(0, -_levelHeight / 2f), new Vector2(_levelWidth + 1f, t));
            CreateWallSegment(walls.transform, new Vector2(-_levelWidth / 2f - t / 2f, 0), new Vector2(t, _levelHeight));
            CreateWallSegment(walls.transform, new Vector2(_levelWidth / 2f + t / 2f, 0), new Vector2(t, _levelHeight));

            return walls;
        }

        private void CreateWallSegment(Transform parent, Vector2 position, Vector2 size)
        {
            var wall = new GameObject("Wall");
            wall.transform.SetParent(parent);
            wall.transform.position = position;
            wall.tag = "Wall";

            var collider = wall.AddComponent<BoxCollider2D>();
            collider.size = size;
        }

        private GameObject CreateGround(Transform parent)
        {
            var ground = new GameObject("Ground");
            ground.transform.SetParent(parent);
            ground.transform.position = new Vector2(0, -_levelHeight / 2f);

            var collider = ground.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(_levelWidth + 1f, 0.5f);

            ground.tag = "Ground";
            return ground;
        }

        private void AddSpawnPoints()
        {
            if (_levelRoot == null) return;
            AddSpawnPointsTo(_levelRoot.transform);
        }

        private void AddSpawnPointsTo(Transform parent)
        {
            var spawnParent = new GameObject("SpawnPoints");
            spawnParent.transform.SetParent(parent);

            for (int i = 0; i < _playerCount; i++)
            {
                var sp = new GameObject($"SpawnPoint_{i + 1}");
                sp.transform.SetParent(spawnParent.transform);
                sp.transform.position = new Vector2(-_levelWidth / 4f + i * 1.5f, -_levelHeight / 4f);

                var gizmo = sp.AddComponent<SpawnPointGizmo>();
                gizmo.playerIndex = i;

                var lm = parent.GetComponent<Core.LevelManager>();
                if (lm != null)
                {
                    System.Array.Resize(ref lm.spawnPoints, i + 1);
                    lm.spawnPoints[i] = sp.transform;
                }
            }
        }

        private void AddExitDoor()
        {
            if (_levelRoot == null) return;

            var doorObj = new GameObject("ExitDoor");
            doorObj.transform.SetParent(_levelRoot.transform);
            doorObj.transform.position = new Vector2(_levelWidth / 4f, 0);
            doorObj.tag = "Door";

            var collider = doorObj.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = new Vector2(1f, 2f);

            var renderer = doorObj.AddComponent<SpriteRenderer>();
            renderer.color = Color.green;

            var door = doorObj.AddComponent<Gameplay.Door>();

            var lm = _levelRoot.GetComponent<Core.LevelManager>();
            if (lm != null) lm.exitDoor = doorObj.transform;

            Selection.activeGameObject = doorObj;
        }

        private void AddKeyPlaceholders()
        {
            if (_levelRoot == null) return;

            for (int i = 0; i < _keyCount; i++)
            {
                var keyObj = new GameObject($"Key_{i + 1}");
                keyObj.transform.SetParent(_levelRoot.transform);
                keyObj.transform.position = new Vector2(Random.Range(-_levelWidth / 3f, _levelWidth / 3f),
                    Random.Range(0, _levelHeight / 3f));
                keyObj.tag = "Key";

                var collider = keyObj.AddComponent<CircleCollider2D>();
                collider.isTrigger = true;
                collider.radius = 0.3f;

                var renderer = keyObj.AddComponent<SpriteRenderer>();
                renderer.color = Color.yellow;

                keyObj.AddComponent<Gameplay.CollectibleKey>();
            }
        }

        private class SpawnPointGizmo : MonoBehaviour
        {
            public int playerIndex;
            private static readonly Color[] GizmoColors = { Color.red, Color.blue, Color.green, Color.yellow };

            private void OnDrawGizmos()
            {
                Color c = playerIndex < GizmoColors.Length ? GizmoColors[playerIndex] : Color.white;
                Gizmos.color = c;
                Gizmos.DrawSphere(transform.position, 0.3f);
                Gizmos.DrawIcon(transform.position + Vector3.up * 0.5f, "GameObject Icon");
            }
        }
    }
}
