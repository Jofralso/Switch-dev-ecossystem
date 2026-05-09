using Game.Player;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class PlayerPrefabBuilder : EditorWindow
    {
        private Color _playerColor = Color.white;
        private bool _addAnimation = true;

        [MenuItem("Game/Create Player Prefab")]
        public static void ShowWindow()
        {
            GetWindow<PlayerPrefabBuilder>("Player Prefab Builder");
        }

        private void OnGUI()
        {
            GUILayout.Label("Player Prefab Configuration", EditorStyles.boldLabel);
            _playerColor = EditorGUILayout.ColorField("Player Color", _playerColor);
            _addAnimation = EditorGUILayout.Toggle("Add Animation Component", _addAnimation);

            GUILayout.Space(10);

            if (GUILayout.Button("Create Player Prefab", GUILayout.Height(30)))
            {
                BuildPlayerPrefab();
            }
        }

        private void BuildPlayerPrefab()
        {
            var playerObj = new GameObject("Player");

            // Visual
            var visualRoot = new GameObject("Visual");
            visualRoot.transform.SetParent(playerObj.transform);

            var body = new GameObject("Body");
            body.transform.SetParent(visualRoot.transform);

            var bodyRenderer = body.AddComponent<SpriteRenderer>();
            bodyRenderer.color = _playerColor;
            bodyRenderer.sortingOrder = 1;

            var hat = new GameObject("Hat");
            hat.transform.SetParent(visualRoot.transform);
            hat.transform.localPosition = Vector3.up * 0.4f;
            var hatRenderer = hat.AddComponent<SpriteRenderer>();
            hatRenderer.sortingOrder = 2;

            // Physics
            var rb = playerObj.AddComponent<Rigidbody2D>();
            rb.gravityScale = 2f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
            rb.freezeRotation = true;

            var collider = playerObj.AddComponent<CircleCollider2D>();
            collider.radius = 0.3f;
            collider.sharedMaterial = new PhysicsMaterial2D
            {
                friction = 0f,
                bounciness = 0f
            };

            // Tags
            playerObj.tag = "Player";

            // Components
            var controller = playerObj.AddComponent<PlayerController>();
            controller.bodyRenderer = bodyRenderer;
            controller.playerColors = new[] { _playerColor };

            playerObj.AddComponent<PlayerInteractor>();

            var setup = playerObj.AddComponent<PlayerSetup>();
            setup.playerColor = _playerColor;

            if (_addAnimation)
            {
                var anim = playerObj.AddComponent<PlayerAnimation>();
                anim.visualRoot = visualRoot.transform;
                anim.bodySprite = bodyRenderer;
                anim.hatSprite = hatRenderer;
            }

            // Save as prefab
            string path = "Assets/Prefabs/Player.prefab";
            System.IO.Directory.CreateDirectory("Assets/Prefabs");

            var prefab = PrefabUtility.SaveAsPrefabAsset(playerObj, path);
            DestroyImmediate(playerObj);

            Selection.activeObject = prefab;
            EditorGUIUtility.PingObject(prefab);

            Debug.Log($"Player prefab created at: {path}");
        }
    }
}
